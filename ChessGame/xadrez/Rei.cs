using ChessGame.tabuleiro;

namespace ChessGame.xadrez
{
    internal class Rei : Peca
    {
        private PartidaXadrez Partida;
        
        public Rei(Tabuleiro tabuleiro, Cor cor, PartidaXadrez partida)
            : base(tabuleiro, cor)
        {
            this.Partida = partida;
        }

        public override string ToString()
        {
            return "R";
        }

        private bool PermiteMover(Posicao posicao)
        {
            Peca peca = Tabuleiro.Peca(posicao);
            return peca == null || peca.Cor != Cor;
        }

        private bool TesteTorreParaRoque(Posicao pos)
        {
            Peca peca = Tabuleiro.Peca(pos);
            return peca != null && peca is Torre && peca.Cor == Cor && peca.QtdMovimentos == 0;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            Posicao pos = new Posicao(0, 0);

            //ACIMA;
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //NORDESTE;
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //DIREITA;
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //SUDESTE;
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //ABAIXO;
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //SUDOESTE;
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //ESQUERDA;
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //NOROESTE;
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);

            if (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            // #JOGADAESPECIAL | ROQUE
            if(QtdMovimentos == 0 && !Partida.Xeque)
            {
                // #JOGADAESPECIAL ROQUE PEQUENO;
                Posicao posicaoTorre1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
                bool torreRoquePequeno = TesteTorreParaRoque(posicaoTorre1);
                if (torreRoquePequeno)
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);

                    if(Tabuleiro.Peca(p1) == null && Tabuleiro.Peca(p2) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }

                // #JOGADAESPECIAL ROQUE GRANDE;
                Posicao posicaoTorre2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                bool torreRoqueGrande = TesteTorreParaRoque(posicaoTorre2);
                if (torreRoqueGrande)
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);

                    if (Tabuleiro.Peca(p1) == null && Tabuleiro.Peca(p2) == null && Tabuleiro.Peca(p3) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }
            }


            return mat;
        }
    }
}
