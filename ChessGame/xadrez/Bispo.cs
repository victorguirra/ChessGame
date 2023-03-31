using ChessGame.tabuleiro;

namespace ChessGame.xadrez
{
    internal class Bispo : Peca
    {
        public Bispo(Tabuleiro tabuleiro, Cor cor)
            : base(tabuleiro, cor) { }

        public override string ToString()
        {
            return "B";
        }

        private bool PermiteMover(Posicao posicao)
        {
            Peca peca = Tabuleiro.Peca(posicao);
            return peca == null || peca.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            Posicao pos = new Posicao(0, 0);

            //NOROESTE;
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            
            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.Linha - 1, pos.Coluna - 1);
            }

            //NORDESTE;
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.Linha - 1, pos.Coluna + 1);
            }

            //SUDESTE;
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.Linha + 1, pos.Coluna + 1);
            }

            //SUDOESTE;
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.Linha + 1, pos.Coluna - 1);
            }

            return mat;
        }
    }
}
