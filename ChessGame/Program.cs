﻿using ChessGame;
using ChessGame.tabuleiro;
using ChessGame.xadrez;

try
{
    PartidaXadrez partidaXadrez = new PartidaXadrez();

    while (!partidaXadrez.Terminada)
    {
        Console.Clear();
        Tela.ImprimirTabuleiro(partidaXadrez.Tabuleiro);

        Console.WriteLine(string.Empty);
        Console.Write("Origem: ");
        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();

        bool[,] posicoesPossiveis = partidaXadrez.Tabuleiro.Peca(origem).MovimentosPossiveis();

        Console.Clear();
        Tela.ImprimirTabuleiro(partidaXadrez.Tabuleiro, posicoesPossiveis);

        Console.WriteLine(string.Empty);
        Console.Write("Destino: ");
        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

        partidaXadrez.ExecutaMovimento(origem, destino);
    }
}
catch(TabuleiroException ex)
{
    Console.WriteLine(ex.Message);
}