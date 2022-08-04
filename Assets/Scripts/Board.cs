public class Board
{
    public ulong white_pieces;
    public ulong black_pieces;

    public bool turn;

    public Board()
    {
        white_pieces = 0x1008000000;
        black_pieces = 0x810000000;
        turn = false;
    }
    public Board(ulong w, ulong b, bool t)
    {
        white_pieces = w;
        black_pieces = b;
        turn = t;
    }

}
