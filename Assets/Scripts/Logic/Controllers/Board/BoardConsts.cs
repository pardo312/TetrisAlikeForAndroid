using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public static class BoardConsts
    {
        public static readonly Color DEFAULT_COLOR = new Color(0.086f, 0.086f, 0.086f, 1);
        public static readonly float OFFSET_TILES =0.1f;
        public static readonly float OFFSET_TILES_NEXT_PIECE_BOARD = 0.3f;


        public const int COLUMNS = 10;
        public const int REAL_ROWS = 20;
        public const int TOTAL_ROWS = REAL_ROWS + 4;
    }
}