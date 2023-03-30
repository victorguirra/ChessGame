using ChessGame.tabuleiro;

namespace ChessGame.xadrez
{
    internal class Rei : Peca
    {
        public Rei(Tabuleiro tabuleiro, Cor cor)
            : base(tabuleiro, cor) { }

        public override string ToString()
        {
            return "R";
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

            return mat;
        }
    }
}
