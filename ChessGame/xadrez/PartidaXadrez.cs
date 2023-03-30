﻿using ChessGame.tabuleiro;

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

        public PartidaXadrez()
        {
            Tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
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

            Cor adversario = Adversario(JogadorAtual);
            bool adversarioEmXeque = EstaEmXeque(adversario);

            if (adversarioEmXeque)
                Xeque = true;
            else
                Xeque = false;
            

            Turno++;
            MudaJogador();
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

            if (!pecaOrigem.PodeMoverPara(destino))
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
        
        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('c', 2, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('d', 2, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('e', 2, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('e', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rei(Tabuleiro, Cor.Branca));

            ColocarNovaPeca('c', 7, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('c', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('d', 7, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('e', 7, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('e', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rei(Tabuleiro, Cor.Preta));
        }
    }
}
