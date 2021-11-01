using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public interface ITile
    {
        int m_tileRow { get; set; }
        int m_tileColumn { get; set; }

        object[] ChangeTileData(object[] _methodParams = null);
        void ResetTile();
    }
}