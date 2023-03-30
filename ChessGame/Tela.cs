using ChessGame.tabuleiro;
using ChessGame.xadrez;

namespace ChessGame
{
    internal class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tabuleiro)
        {
            for (int i = 0; i < tabuleiro.Linhas; i++)
            {
                Console.Write($"{8 - i} ");

                for (int j = 0; j < tabuleiro.Colunas; j++)
                {
                    Peca peca = tabuleiro.Peca(i, j);
                    ImprimirPeca(peca);
                }
                Console.WriteLine(string.Empty);
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirTabuleiro(Tabuleiro tabuleiro, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tabuleiro.Linhas; i++)
            {
                Console.Write($"{8 - i} ");

                for (int j = 0; j < tabuleiro.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                        Console.BackgroundColor = fundoAlterado;
                    else
                        Console.BackgroundColor = fundoOriginal;

                    Peca peca = tabuleiro.Peca(i, j);
                    ImprimirPeca(peca);
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine(string.Empty);
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
                Console.Write("- ");
            else
            {
                if (peca.Cor == Cor.Branca)
                    Console.Write(peca);
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string entrada = Console.ReadLine();
            char coluna = entrada[0];
            int linha = int.Parse(entrada[1].ToString());

            return new PosicaoXadrez(coluna, linha);
        }
    }
}
