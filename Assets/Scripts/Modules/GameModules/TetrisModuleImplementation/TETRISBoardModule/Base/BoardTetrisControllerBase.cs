using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public abstract class BoardTetrisControllerBase : BoardControllerBase<HideableTileBase>
    {
        public virtual void SpawnPiece(int _pieceRows, int _pieceColumns, Piece _nextPiece, Action<Piece, Vector2Int, List<Vector2Int>> callback = null)
        {
            for (int i = _pieceRows - 4; i < _pieceRows; i++)
            {
                for (int j = _pieceRows - 4; j < _pieceColumns; j++)
                {
                    if (_nextPiece.pieceForms[0].pieceTiles[(3 - i) + ((j) * PieceForm.PIECE_TILES_WIDTH)])
                        m_board[i, j].ChangeTileData(new object[2] { _nextPiece.pieceColor, null });
                    else
                        m_board[i, j].ChangeTileData(new object[2] { PieceConsts.DEFAULT_COLOR, null });
                }
            }
        }
    }
}