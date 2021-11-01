using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public abstract class BoardTetrisControllerBase : BoardControllerBase<HideableTileBase>
    {
        public virtual void SpawnPiece(int _realRows, Piece _nextPiece, Action<Piece, Vector2Int, List<Vector2Int>> callback = null)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (nextPiece.pieceForms[0].pieceTiles[(3 - i) + ((j) * PieceForm.PIECE_TILES_WIDTH)])
                        m_board[i, j].ChangeTileData(new object[2] { nextPiece.pieceColor, null });
                    else
                        m_board[i, j].ChangeTileData(new object[2] { PieceConsts.DEFAULT_COLOR, null });
                }
            }
        }
    }
}