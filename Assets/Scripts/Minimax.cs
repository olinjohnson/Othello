using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    public Game game;
    public int depth;

    public uint nodes_searched;
    public int[] position_offsets = new int[]
    {
        15, 5, 5, 5, 5, 5, 5, 15,
        5, 0, 0, 0, 0, 0, 0, 5,
        5, 0, 0, 0, 0, 0, 0, 5,
        5, 0, 0, 0, 0, 0, 0, 5,
        5, 0, 0, 0, 0, 0, 0, 5,
        5, 0, 0, 0, 0, 0, 0, 5,
        5, 0, 0, 0, 0, 0, 0, 5,
        15, 5, 5, 5, 5, 5, 5, 15,
    };

    public Minimax(int d)
    {
        game = new Game();
        depth = d;
    }

    public int FindIdealMove(Board b)
    {
        // White(AI) trying to maximize score, Black(Player) trying to minimize
        nodes_searched = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();

        ulong valid_moves = game.GetValidMoves(b);
        int most = -10000;
        int[] scores = new int[64];
        Array.Fill(scores, -10000);

        for (int i = 0; i < 64; i++)
        {
            if ((valid_moves & ((ulong)1 << i)) > 0)
            {
                int val = -Run(game.MakeMove(b, i), depth - 1, -1) + position_offsets[i];
                most = Math.Max(most, val);
                scores[i] = val;
            }
        }

        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        PlayerPrefs.nodesSearched = nodes_searched;
        PlayerPrefs.timeSearched = elapsed;

        return Array.IndexOf(scores, most);
    }

    public int Run(Board ghostMove, int d, int color)
    {
        nodes_searched++;
        // Check for terminal state
        if(game.DetectEnding(ghostMove))
        {
            return (Evaluate(ghostMove, d) + (d * color)) * 100;
        }
        // Check if max depth reached
        else if(d == 0)
        {
            return Evaluate(ghostMove, d) + (d * color);
        }
        else
        {
            ulong valid_moves = game.GetValidMoves(ghostMove);
            // If no valid moves
            if(valid_moves == 0)
            {
                return -Run(new Board(ghostMove.white_pieces, ghostMove.black_pieces, !ghostMove.turn), d - 1, -color);
            }
            else
            {
                int value = -10000;
                for (int i = 0; i < 64; i++)
                {
                    if ((valid_moves & ((ulong)1 << i)) > 0)
                    {
                        int child = -Run(game.MakeMove(ghostMove, i), d - 1, -color) + position_offsets[i];
                        value = Math.Max(value, child);
                    }
                }
                return value;
            }
        }
    }

    int Evaluate(Board b, int current_d)
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

        return (w_count - b_count);
    }
}
