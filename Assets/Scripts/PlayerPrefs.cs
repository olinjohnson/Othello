using System;

public class PlayerPrefs
{
    public static int searchDepth = 10;
    public static bool showStats = false;

    public static bool AIOpponent = true;

    public static ulong nodesSearched { get; set; }
    public static long timeSearched { get; set; }
}
