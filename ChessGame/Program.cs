using ChessGame;
using ChessGame.tabuleiro;
using ChessGame.xadrez;

try
{
    PartidaXadrez partidaXadrez = new PartidaXadrez();

    while (!partidaXadrez.Terminada)
    {
        try
        {
            Console.Clear();
            Tela.ImprimirPartida(partidaXadrez);

            Console.WriteLine(string.Empty);
            Console.Write("Origem: ");
            Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
            partidaXadrez.ValidarPosicaoOrigem(origem);

            bool[,] posicoesPossiveis = partidaXadrez.Tabuleiro.Peca(origem).MovimentosPossiveis();

            Console.Clear();
            Tela.ImprimirTabuleiro(partidaXadrez.Tabuleiro, posicoesPossiveis);

            Console.WriteLine(string.Empty);
            Console.Write("Destino: ");
            Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
            partidaXadrez.ValidarPosicaoDestino(origem, destino);

            partidaXadrez.RealizaJogada(origem, destino);
        }
        catch(TabuleiroException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Pressione Enter para refazer a jogada!");
            Console.ReadLine();
        }
    }
        
}
catch(TabuleiroException ex)
{
    Console.WriteLine(ex.Message);
}