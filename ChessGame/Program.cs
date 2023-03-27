using ChessGame;
using ChessGame.tabuleiro;
using ChessGame.xadrez;

try
{
    Tabuleiro tabuleiro = new Tabuleiro(8, 8);

    tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Preta), new Posicao(0, 0));
    tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Preta), new Posicao(1, 3));
    tabuleiro.ColocarPeca(new Rei(tabuleiro, Cor.Preta), new Posicao(2, 4));
    tabuleiro.ColocarPeca(new Rei(tabuleiro, Cor.Preta), new Posicao(0, 2));
    
    Tela.ImprimirTabuleiro(tabuleiro);
}
catch(TabuleiroException ex)
{
    Console.WriteLine(ex.Message);
}