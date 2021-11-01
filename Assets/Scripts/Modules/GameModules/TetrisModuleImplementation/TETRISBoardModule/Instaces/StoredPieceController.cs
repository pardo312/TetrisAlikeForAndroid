using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class StoredPieceController : BoardTetrisControllerBase 
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
            base.CreateBoard(_rows, _columns, _offsetTiles, _createdTile);
            _createdTile += (row, column) =>
            {
                m_board[row, column].m_tileRow = row;
                m_board[row, column].m_tileColumn = column;
            };
        }

        public Piece StorePiece(Piece piece)
        {
            Piece deStoredPiece = null;
            if (m_queueofStored.Count > 0)
                deStoredPiece = m_queueofStored.Dequeue();

            m_queueofStored.Enqueue(piece);
            ShowStoredPiece(piece);

            return deStoredPiece;
        }

        private void ShowStoredPiece(Piece storedPiece)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (storedPiece.pieceForms[0].pieceTiles[(3 - i) + ((j) * PieceForm.PIECE_TILES_WIDTH)])
                        m_board[i, j].ChangeTileData(new object[2] { storedPiece.pieceColor, null });
                    else
                        m_board[i, j].ChangeTileData(new object[2] { PieceConsts.DEFAULT_COLOR, null });
                }
            }
        }

        #endregion Methods
    }
}