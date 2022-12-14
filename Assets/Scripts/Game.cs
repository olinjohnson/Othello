using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    // Mask order: N, W, NW, SW, S, E, SE, NE
    public ulong[] direction_masks = { 0x7F7F7F7F7F7F7F7F, 0x00FFFFFFFFFFFFFF, 0x007F7F7F7F7F7F7F, 0x00FEFEFEFEFEFEFE, 0xFEFEFEFEFEFEFEFE, 0xFFFFFFFFFFFFFF00, 0xFEFEFEFEFEFEFE00, 0x7F7F7F7F7F7F7F00};
    public int[] direction_offsets = { 1, 8, 9, 7 };

    public bool DetectEnding(Board b)
    {
        if((b.white_pieces | b.black_pieces) == 0xFFFFFFFFFFFFFFFF)
        {
            return true;
        }

        if(GetValidMoves(b) == 0 && GetValidMoves(new Board(b.white_pieces, b.black_pieces, !b.turn)) == 0)
        {
            return true;
        }

        return false;
    }

    public int getWinner(Board b)
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

        if(w_count > b_count)
        {
            return 0;
        }
        else if(b_count > w_count)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public ulong GetValidMoves(Board b)
    {
        ulong valid_moves = 0;

        ulong empty_tiles = ~(b.black_pieces | b.white_pieces);
        ulong pPieces, oPieces;
        if (b.turn)
        {
            pPieces = b.black_pieces;
            oPieces = b.white_pieces;
        }
        else
        {
            pPieces = b.white_pieces;
            oPieces = b.black_pieces;
        }
        for (int i = 0; i < 4; i++)
        {
            ulong neighbor = ((pPieces & direction_masks[i]) << direction_offsets[i]) & oPieces;

            while(neighbor > 0)
            {
                ulong acreage = (neighbor & direction_masks[i]) << direction_offsets[i];
                valid_moves |= acreage & empty_tiles;
                neighbor = acreage & oPieces;
            }

            neighbor = ((pPieces & direction_masks[i + 4]) >> direction_offsets[i]) & oPieces;

            while (neighbor > 0)
            {
                ulong acreage = (neighbor & direction_masks[i + 4]) >> direction_offsets[i];
                valid_moves |= acreage & empty_tiles;
                neighbor = acreage & oPieces;
            }
        }

        return valid_moves;
    }

    public Board MakeMove(Board b, int m)
    {
        // Determine player and opponent pieces
        ulong pPieces, oPieces;
        if(b.turn)
        {
            pPieces = b.black_pieces;
            oPieces = b.white_pieces;
        }
        else
        {
            pPieces = b.white_pieces;
            oPieces = b.black_pieces;
        }

        // Flip pieces
        ulong mask = (ulong)1 << m;
        pPieces |= mask;

        // Execute flip rays
        // Room for optimization
        ulong flippers = 0;
        for(int i = 0; i < 4; i++)
        {
            ulong acreage = (mask & direction_masks[i]) << direction_offsets[i];
            ulong temp_flippers = 0;
            
            while ((acreage & oPieces) > 0)
            {
                temp_flippers |= acreage & oPieces;
                acreage = (acreage & direction_masks[i]) << direction_offsets[i];
            }
            if((acreage & pPieces) > 0)
            {
                flippers |= temp_flippers;
            }

            acreage = (mask & direction_masks[i + 4]) >> direction_offsets[i];
            temp_flippers = 0;

            while ((acreage & oPieces) > 0)
            {
                temp_flippers |= acreage & oPieces;
                acreage = (acreage & direction_masks[i + 4]) >> direction_offsets[i];
            }
            if ((acreage & pPieces) > 0)
            {
                flippers |= temp_flippers;
            }
        }
        oPieces ^= flippers;
        pPieces |= flippers;

        return b.turn ? new Board(oPieces, pPieces, false) : new Board(pPieces, oPieces, true);
    }

    public Board MakeAIMove(Board b)
    {
        if(!b.turn)
        {
            Minimax algo = new Minimax(PlayerPrefs.searchDepth);
            int idealMove = algo.FindIdealMove(b);
            return MakeMove(b, idealMove);
        }
        throw new ArgumentException();
    }
}
