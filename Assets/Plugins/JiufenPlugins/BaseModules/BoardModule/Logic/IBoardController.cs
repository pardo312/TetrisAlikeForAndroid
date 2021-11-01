using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public interface IBoardController<T>
    {
        GameObject m_tilePrefab { get; }
        Transform m_tileParent { get; }
        T[,] m_board { get; }

        void Init();
        void CreateBoard(int _rows, int _columns,float offset = 1, Action<int,int> createdTile = null);
    }
}