namespace ChessGame.tabuleiro
{
    internal class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas { get; set; }

        public Tabuleiro(int linhas, int colunas)
        {
            this.Linhas = linhas;
            this.Colunas = colunas;
            Pecas = new Peca[linhas, colunas];
        }

        public Peca Peca(int linha, int coluna)
        {
            return Pecas[linha, coluna];
        }

        public Peca Peca(Posicao pos)
        {
            return Pecas[pos.Linha, pos.Coluna];
        }

        public bool ExistePeca(Posicao posicao)
        {
            ValidarPosicao(posicao);

            return Peca(posicao) != null;
        }

        public void ColocarPeca(Peca peca, Posicao posiscao)
        {
            bool existePeca = ExistePeca(posiscao);

            if (existePeca)
                throw new TabuleiroException("Já existe uma peça nessa posição");

            Pecas[posiscao.Linha, posiscao.Coluna] = peca;
            peca.Posicao = posiscao;
        }

        public bool PosicaoValida(Posicao posicao)
        {
            bool posicaoValida =
                posicao.Linha < 0 || posicao.Linha >= Linhas ||
                posicao.Coluna < 0 || posicao.Linha >= Colunas;

            return !posicaoValida;
        }

        public void ValidarPosicao(Posicao posicao)
        {
            bool posicaoValida = PosicaoValida(posicao);
            
            if (!posicaoValida)
                throw new TabuleiroException("Posição Inválida!");
        }
    }
}
