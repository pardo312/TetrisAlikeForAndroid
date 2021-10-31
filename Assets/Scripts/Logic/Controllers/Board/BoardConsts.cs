using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public static class BoardConsts
    {
        public static readonly float OFFSET_TILES = 0.9f;
        public static readonly float OFFSET_TILES_NEXT_PIECE_BOARD = 0.6f;

        public const int COLUMNS = 10;
        public const int REAL_ROWS = 20;
        public const int TOTAL_ROWS = REAL_ROWS + 4;
    }
}