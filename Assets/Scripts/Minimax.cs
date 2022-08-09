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
    public int depth;

    public Minimax(int d)
    {
        game = new Game();
        depth = d;
    }

    public int FindIdealMove(Board b)
    {

    }

    public int Run(Board ghostMove, int d, int current_d)
    {
        // Check for terminal state
        if(game.DetectEnding(ghostMove))
        {
            return Evaluate(ghostMove) * 100;
        }
        else if(d == current_d)
        {
            return Evaluate(ghostMove);
        }

        // Expand leaf nodes and return
        ulong valid_moves = game.GetValidMoves(ghostMove);
        for(int i = 0; i < 64; i++)
        {
            if((valid_moves & ((ulong)1 << i)) > 0)
            {
                int child = Run(game.MakeMove(ghostMove, i), d, current_d + 1);
                if(ghostMove.turn)
                {

                }
            }
        }
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

        return b_count - w_count;
    }
}
