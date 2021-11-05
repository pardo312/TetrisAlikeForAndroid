using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class BoardController : BoardTetrisControllerBase
    {
        #region Init
        public override void Init()
        {
            //3 EmptyRows To handle final round pieces.
            CreateBoard(BoardConsts.TOTAL_ROWS, BoardConsts.COLUMNS, BoardConsts.OFFSET_TILES);
        }

        public override void CreateBoard(int _rows, int _columns, float _offsetTiles = 1, Action<int, int> _createdTile = null)
        {
            base.CreateBoard(_rows, _columns, _offsetTiles, (row, column) =>
            {
                m_board[row, column].m_tileRow = row;
                m_board[row, column].m_tileColumn = column;
                if (row == BoardConsts.REAL_ROWS)
                    m_board[row, column].SetFirstHiddenRowPiece();
                if (row > BoardConsts.REAL_ROWS)
                    m_board[row, column].SetPieceToBeHidden();
            });
        }
        #endregion Init

        #region Spawn
        public override void SpawnPiece(int _pieceRows, int _pieceColumns, Piece _nextPiece, Action<Piece, Vector2Int, List<Vector2Int>> callback = null)
        {
            List<Vector2Int> currentPieceTiles = new List<Vector2Int>();

            int offset = 0;
            //Spawn Piece in the upper 4x4 space of the board
            for (int i = _pieceRows - 4; i < _pieceRows; i++)
            {
                bool rowFilled = false;
                for (int j = _pieceColumns - 3; j <= _pieceColumns; j++)
                    if (((TetrisTileData)m_board[i, j].m_tileData).IsFilled)
                        rowFilled = true;

                if (rowFilled)
                    offset++;
            }

            Vector2Int piece4x4SquareTiles = new Vector2Int(_pieceRows - 4, 3);
            for (int i = _pieceRows - 4; i < _pieceRows; i++)
            {
                for (int j = 3; j <= 6; j++)
                    if (_nextPiece.pieceForms[0].pieceTiles[((_pieceRows - 1) - i) + ((j - 3) * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        piece4x4SquareTiles = new Vector2Int(i + offset-3 , 3);
                        m_board[i + offset, j].ChangeTileData(new object[2] { _nextPiece.pieceColor, null });
                        currentPieceTiles.Add(new Vector2Int(i + offset, j));
                    }
            }
            callback?.Invoke(_nextPiece, piece4x4SquareTiles, currentPieceTiles);
        }
        #endregion Spawn

        #region Line manipulation
        public void ClearCompletedLine(List<int> filledRows)
        {
            for (int i = filledRows.Count - 1; i >= 0; i--)
            {
                ResetLine(filledRows[i]);
                DropUpperLinesOfCurrentLine(filledRows[i]);
            }
        }

        private void ResetLine(int row)
        {
            for (int i = 0; i < BoardConsts.COLUMNS; i++)
                m_board[row, i].ResetTile();
        }

        private void DropUpperLinesOfCurrentLine(int row)
        {
            for (int i = row + 1; i < BoardConsts.REAL_ROWS; i++)
            {
                DropLine(i);
                ResetLine(i);
            }
        }

        private void DropLine(int currentLine)
        {
            for (int j = 0; j < BoardConsts.COLUMNS; j++)
            {
                TetrisTileData tileData = m_board[currentLine, j].m_tileData as TetrisTileData;
                m_board[currentLine - 1, j].ChangeTileData(new object[2] { tileData.Color, null });

                if (tileData.IsFilled)
                    m_board[currentLine - 1, j].ChangeTileData(new object[2] { null, true });
            }
        }
        #endregion Line manipulation

    }
}