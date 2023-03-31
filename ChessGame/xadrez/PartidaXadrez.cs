using ChessGame.tabuleiro;

namespace ChessGame.xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro Tabuleiro { get; set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> PecasCapturadas;
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaXadrez()
        {
            Tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca peca = Tabuleiro.RetirarPeca(origem);
            peca.IncrementarQtdMovimentos();

            Peca pecaCapturada = Tabuleiro.RetirarPeca(destino);
            Tabuleiro.ColocarPeca(peca, destino);

            if (pecaCapturada != null)
                PecasCapturadas.Add(pecaCapturada);

            // #JOGADAESPECIAL | ROQUE PEQUENO;
            if(peca is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                
                Peca torre = Tabuleiro.RetirarPeca(origemTorre);
                torre.IncrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(torre, destinoTorre);
            }

            // #JOGADAESPECIAL | ROQUE GRANDE;
            if (peca is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);

                Peca torre = Tabuleiro.RetirarPeca(origemTorre);
                torre.IncrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(torre, destinoTorre);
            }

            // #JOGADAESPECIAL | EN PASSANT;
            if(peca is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posicaoPeao;
                    if (peca.Cor == Cor.Branca)
                        posicaoPeao = new Posicao(destino.Linha + 1, destino.Coluna);
                    else
                        posicaoPeao = new Posicao(destino.Linha - 1, destino.Coluna);

                    pecaCapturada = Tabuleiro.RetirarPeca(posicaoPeao);
                    PecasCapturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            bool estaEmXeque = EstaEmXeque(JogadorAtual);
            if (estaEmXeque)
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca peca = Tabuleiro.Peca(destino);

            // #JOGADAESPECIAL | PROMOCAO;
            if(peca is Peao)
            {
                if(
                    (peca.Cor == Cor.Branca && destino.Linha == 0)
                    ||
                    (peca.Cor == Cor.Preta && destino.Linha == 7)
                )
                {
                    peca = Tabuleiro.RetirarPeca(destino);
                    Pecas.Remove(peca);

                    Peca dama = new Dama(Tabuleiro, peca.Cor);
                    Tabuleiro.ColocarPeca(dama, destino);
                    Pecas.Add(dama);
                }
            }

            Cor adversario = Adversario(JogadorAtual);
            bool adversarioEmXeque = EstaEmXeque(adversario);

            if (adversarioEmXeque)
                Xeque = true;
            else
                Xeque = false;

            bool xequeMate = XequeMate(adversario);
            if (xequeMate)
                Terminada = true;
            else
            {
                Turno++;
                MudaJogador();
            }

            // #JOGADAESPECIAL | EN PASSANT
            if(peca is Peao &&
                (destino.Linha == origem.Linha -2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = peca;
            }
            else
            {
                VulneravelEnPassant = null;
            }
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca peca = Tabuleiro.RetirarPeca(destino);
            peca.DecrementarQtdMovimentos();
            
            if(pecaCapturada != null)
            {
                Tabuleiro.ColocarPeca(pecaCapturada, destino);
                PecasCapturadas.Remove(pecaCapturada);
            }

            Tabuleiro.ColocarPeca(peca, origem);

            // #JOGADAESPECIAL | ROQUE PEQUENO;
            if (peca is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);

                Peca torre = Tabuleiro.RetirarPeca(destinoTorre);
                torre.DecrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(torre, origemTorre);
            }

            // #JOGADAESPECIAL | ROQUE GRANDE;
            if (peca is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);

                Peca torre = Tabuleiro.RetirarPeca(destinoTorre);
                torre.DecrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(torre, origemTorre);
            }

            // #JOGADAESPECIAL | EN PASSANT;
            if(peca is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tabuleiro.RetirarPeca(destino);
                    Posicao posicaoPeao;

                    if (peao.Cor == Cor.Branca)
                        posicaoPeao = new Posicao(3, destino.Coluna);
                    else
                        posicaoPeao = new Posicao(4, destino.Coluna);

                    Tabuleiro.ColocarPeca(peao, posicaoPeao);
                }
            }
        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            Peca peca = Tabuleiro.Peca(pos);

            if (peca == null)
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");

            if (JogadorAtual != peca.Cor)
                throw new TabuleiroException("A peça de origem escolhida não é sua!");

            if(!peca.ExisteMovimentosPossiveis())
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = Tabuleiro.Peca(origem);

            if (!pecaOrigem.MovimentoPossivel(destino))
                throw new TabuleiroException("Posição de destino inválida!");
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preta;
            else
                JogadorAtual = Cor.Branca;
        }

        public HashSet<Peca> ObterPecasCapturadas(Cor cor)
        {
            return PecasCapturadas.Where(x => x.Cor == cor).ToHashSet();
        }

        public HashSet<Peca> ObterPecas(Cor cor)
        {
            HashSet<Peca> pecasCapturadas = ObterPecasCapturadas(cor);

            HashSet<Peca> pecas = Pecas.Where(x => x.Cor == cor).ToHashSet();
            pecas.ExceptWith(pecasCapturadas);

            return pecas;
        }

        private Cor Adversario(Cor cor)
        {
            if (cor == Cor.Branca)
                return Cor.Preta;
            else
                return Cor.Branca;
        }

        private Peca Rei(Cor cor)
        {
            HashSet<Peca> pecasEmJogo = ObterPecas(cor);
            
            foreach(Peca peca in pecasEmJogo)
                if(peca is Rei) return peca;

            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca rei = Rei(cor);

            if (rei == null)
                throw new TabuleiroException($"Não existe rei da cor {cor} no tabuleiro!");

            Cor adversario = Adversario(cor);
            HashSet<Peca> pecasEmJogo = ObterPecas(adversario);

            foreach(Peca peca in pecasEmJogo)
            {
                bool[,] movimentosPossiveis = peca.MovimentosPossiveis();
                bool xeque = movimentosPossiveis[rei.Posicao.Linha, rei.Posicao.Coluna];

                if (xeque)
                    return true;
            }

            return false;
        }
        
        public bool XequeMate(Cor cor)
        {
            bool xeque = EstaEmXeque(cor);
            if (!xeque)
                return false;

            HashSet<Peca> pecasEmJogo = ObterPecas(cor);
            foreach(Peca peca in pecasEmJogo)
            {
                bool[,] movimentosPossiveis = peca.MovimentosPossiveis();
                for(int i = 0; i < Tabuleiro.Linhas; i++)
                {
                    for(int j = 0; j < Tabuleiro.Colunas; j++)
                    {
                        bool posicaoPossivel = movimentosPossiveis[i, j];
                        if (posicaoPossivel)
                        {
                            Posicao origem = peca.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);

                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tabuleiro, Cor.Branca, this));

            ColocarNovaPeca('a', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tabuleiro, Cor.Preta, this));
        }
    }
}
