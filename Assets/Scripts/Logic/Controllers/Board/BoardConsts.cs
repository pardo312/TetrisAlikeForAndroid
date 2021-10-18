using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public static class BoardConsts
    {
        public static readonly Color DEFAULT_COLOR = new Color(0.086f, 0.086f, 0.086f, 1);

        public const int COLUMNS = 10;
        public const int REAL_ROWS = 20;
        public const int TOTAL_ROWS = REAL_ROWS + 4;
    }
}