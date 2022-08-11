using System;
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

    public uint nodes_searched;

    public Minimax(int d)
    {
        game = new Game();
        depth = d;
    }

    public int FindIdealMove(Board b)
    {
        nodes_searched = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();

        ulong valid_moves = game.GetValidMoves(b);
        int least = 1000;
        int[] scores = new int[64];
        for (int i = 0; i < 64; i++)
        {
            if ((valid_moves & ((ulong)1 << i)) > 0)
            {
                int val = Run(game.MakeMove(b, i), depth, 1);
                least = Math.Min(least, val);
                scores[i] = val;
            }
        }

        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Debug.Log($"Searched: {nodes_searched} --- Elapsed: {elapsed}ms");

        return Array.IndexOf(scores, least);
    }

    public int Run(Board ghostMove, int d, int current_d)
    {
        nodes_searched++;
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
        int value = ghostMove.turn ? -1000 : 1000;
        for(int i = 0; i < 64; i++)
        {
            if((valid_moves & ((ulong)1 << i)) > 0)
            {
                int child = Run(game.MakeMove(ghostMove, i), d, current_d + 1);
                // Player turn - MAX
                if(ghostMove.turn)
                {
                    value = Math.Max(value, child);
                }
                // AI turn - MIN
                else
                {
                    value = Math.Min(value, child);
                }
            }
        }
        return value;
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
