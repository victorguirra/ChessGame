using ChessGame.tabuleiro;

namespace ChessGame
{
    internal class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tabuleiro)
        {
            for (int i = 0; i < tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < tabuleiro.Colunas; j++)
                {
                    Peca peca = tabuleiro.Peca(i, j);
                    string mensagem = string.Empty;

                    if (peca == null)
                        mensagem = "- ";
                    else
                        mensagem = $"{peca} ";
                    
                    Console.Write(mensagem);
                }
                Console.WriteLine(string.Empty);
            }
        }
    }
}
