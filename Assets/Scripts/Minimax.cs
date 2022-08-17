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
        -15, -5, -5, -5, -5, -5, -5, -15,
        -5, 0, 0, 0, 0, 0, 0, -5,
        -5, 0, 0, 0, 0, 0, 0, -5,
        -5, 0, 0, 0, 0, 0, 0, -5,
        -5, 0, 0, 0, 0, 0, 0, -5,
        -5, 0, 0, 0, 0, 0, 0, -5,
        -5, 0, 0, 0, 0, 0, 0, -5,
        -15, -5, -5, -5, -5, -5, -5, -15,
    };

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
        int least = 10000;
        int alpha = -10000;
        int beta = 10000;
        int[] scores = new int[64];
        Array.Fill(scores, 10000);

        for (int i = 0; i < 64; i++)
        {
            if ((valid_moves & ((ulong)1 << i)) > 0)
            {
                int val = Run(game.MakeMove(b, i), depth, 1, alpha, beta) + position_offsets[i];
                least = Math.Min(least, val);
                scores[i] = val;
            }
        }

        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        PlayerPrefs.nodesSearched = nodes_searched;
        PlayerPrefs.timeSearched = elapsed;

        return Array.IndexOf(scores, least);
    }

    public int Run(Board ghostMove, int d, int current_d, int alpha, int beta)
    {
        nodes_searched++;
        // Check for terminal state
        if(game.DetectEnding(ghostMove))
        {
            return Evaluate(ghostMove, current_d) * 100;
        }
        // Check if max depth reached
        else if(d == current_d)
        {
            return Evaluate(ghostMove, current_d);
        }
        else
        {
            ulong valid_moves = game.GetValidMoves(ghostMove);
            // If no valid moves
            if(valid_moves == 0)
            {
                return Run(new Board(ghostMove.white_pieces, ghostMove.black_pieces, !ghostMove.turn), d, current_d + 1, alpha, beta);
            }
            else
            {
                // Expand leaf nodes and return
                int value = ghostMove.turn ? -10000 : 10000;
                for (int i = 0; i < 64; i++)
                {
                    if ((valid_moves & ((ulong)1 << i)) > 0)
                    {

                        // Player turn - MAX 
                        if (ghostMove.turn)
                        {
                            int child = Run(game.MakeMove(ghostMove, i), d, current_d + 1, alpha, beta) + Math.Abs(position_offsets[i]);
                            value = Math.Max(value, child);
                            alpha = Math.Max(alpha, value);
                            if(alpha >= beta) { break; }
                        }
                        // AI turn - MIN
                        else
                        {
                            int child = Run(game.MakeMove(ghostMove, i), d, current_d + 1, alpha, beta) + position_offsets[i];
                            value = Math.Min(value, child);
                            beta = Math.Min(beta, value);
                            if (alpha >= beta) { break; }
                        }
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

        return (b_count - w_count) + current_d;
    }
}
