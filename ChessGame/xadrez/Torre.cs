using ChessGame.tabuleiro;

namespace ChessGame.xadrez
{
    internal class Torre : Peca
    {
        public Torre(Tabuleiro tabuleiro, Cor cor)
            : base(tabuleiro, cor) { }

        public override string ToString()
        {
            return "T";
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

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.Linha = pos.Linha - 1;
            }

            //ABAIXO;
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.Linha = pos.Linha + 1;
            }

            //DIREITA;
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.Coluna = pos.Coluna + 1;
            }

            //ESQUERDA;
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);

            while (Tabuleiro.PosicaoValida(pos) && PermiteMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                Peca peca = Tabuleiro.Peca(pos);
                if (peca != null && peca.Cor != Cor)
                    break;

                pos.Coluna = pos.Coluna - 1;
            }

            return mat;
        }
    }
}
