using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    /*
     * The player (black), is trying to maximize the score,
     * While the computer (white) tries to minimize it.
     */

    public Game game;

    public Minimax()
    {
        game = new Game();
    }


    int Evaluate(Board b)
    {
        // Count set bits
        int w_count = 0;
        int b_count = 0;
        ulong black = b.black_pieces;
        ulong white = b.white_pieces;

        while (black > 0)
        {
            b_count += (int)(black & 1);
            black >>= 1;
        }
        while (white > 0)
        {
            w_count += (int)(white & 1);
            white >>= 1;
        }

        if (game.DetectEnding(b))
        {
            return (b_count - w_count) * 100;
        }
        else
        {
            return (b_count - w_count);
        }
    }
}
