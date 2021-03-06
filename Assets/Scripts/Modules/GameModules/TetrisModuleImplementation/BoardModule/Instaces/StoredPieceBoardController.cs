using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class StoredPieceBoardController : BoardTetrisControllerBase 
    {
        #region Fields
        //Data
        private Queue<Piece> m_queueofStored = new Queue<Piece>();
        #endregion Fields

        #region Methods
        public override void Init()
        {
            CreateBoard(4,4,BoardConsts.OFFSET_TILES_NEXT_PIECE_BOARD);
        }

        public override void CreateBoard(int _rows, int _columns,float _offsetTiles = 1,Action<int, int> _createdTile = null)
        {
            base.CreateBoard(_rows, _columns, _offsetTiles, (row, column) =>
            {
                m_board[row, column].m_tileRow = row;
                m_board[row, column].m_tileColumn = column;
            });
        }

        public Piece StorePiece(Piece piece)
        {
            Piece deStoredPiece = null;
            if (m_queueofStored.Count > 0)
                deStoredPiece = m_queueofStored.Dequeue();

            m_queueofStored.Enqueue(piece);
            SpawnPiece(4, 4, piece);

            return deStoredPiece;
        }
        #endregion Methods
    }
}