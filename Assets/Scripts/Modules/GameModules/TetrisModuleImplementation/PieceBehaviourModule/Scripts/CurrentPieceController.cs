using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class CurrentPieceController
    {
        #region Fields

        public Piece m_currentPiece;
        private int m_currentPieceFormIndex = 0;
        [HideInInspector] public List<Vector2Int> m_currentPieceTiles;
        [HideInInspector] public Vector2Int[] m_currentProjectionPieces = new Vector2Int[4];
        public Vector2Int m_piece4x4CubeStartTile;

        #region References

        private BoardController m_boardController;

        #endregion References

        #endregion Fields

        #region Methods

        #region Init

        public void Init(BoardController boardController)
        {
            m_boardController = boardController;
        }

        public void OnSpawn()
        {
            m_currentPieceFormIndex = 0;
        }

        #endregion Init

        #region See Where piece is going to drop

        public void SeeWhereCurrentPieceIsDropping()
        {
            ClearCurrentPieceTiles(m_currentProjectionPieces, m_currentPieceTiles);
            Vector2Int[] tempCurrentPiecesTiles = new Vector2Int[m_currentPieceTiles.Count];
            m_currentPieceTiles.CopyTo(tempCurrentPiecesTiles);

            CheckLowestNotFilledTile(tempCurrentPiecesTiles, (lowestNotFilledTile, lowestRowInCurrentPiece) =>
             {
                 for (int j = tempCurrentPiecesTiles.Length - 1; j >= 0; j--)
                 {
                     int nextRowForThisTile = lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece);

                     //Search if the preview tile is blocking any current tile
                     bool tileHasCurrentPiece = false;

                     for (int j2 = tempCurrentPiecesTiles.Length - 1; j2 >= 0; j2--)
                         if (nextRowForThisTile == tempCurrentPiecesTiles[j2].x && tempCurrentPiecesTiles[j].y == tempCurrentPiecesTiles[j2].y)
                             tileHasCurrentPiece = true;

                     if (tileHasCurrentPiece)
                     {
                         m_boardController._board[nextRowForThisTile, tempCurrentPiecesTiles[j].y].ChangeTileData(new object[2] { m_currentPiece.pieceColor, null });
                         continue;
                     }

                     //Show Preview tile
                     if (!((TetrisTileData)m_boardController._board[lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece), tempCurrentPiecesTiles[j].y].m_tileData).IsFilled)
                     {
                         Debug.LogWarning("Jiufen if warning check here.");
                         Color lightVersionColor = m_currentPiece.pieceColor;
                         lightVersionColor.a = 0.1f;
                         m_boardController._board[nextRowForThisTile, tempCurrentPiecesTiles[j].y].ChangeTileData(new object[2] { lightVersionColor, null });
                         m_currentProjectionPieces[j] = new Vector2Int(nextRowForThisTile, tempCurrentPiecesTiles[j].y);
                     }
                 }
             });
        }

        #endregion See Where piece is going to drop

        #region Interaction with other pieces

        /// <summary>
        /// When the current piece drops to the lowest possition, check if the tile bellow is inside te tetris board. If not end the game
        /// </summary>
        /// <param name="_shouldSpawnNewPiece"></param>
        /// <returns>Filled rows</returns>
        public List<int> CheckTileBelow(ref bool _shouldSpawnNewPiece)
        {
            List<int> filledRows = new List<int>();
            for (int m = m_currentPieceTiles.Count - 1; m >= 0; m--)
            {
                m_boardController._board[m_currentPieceTiles[m].x, m_currentPieceTiles[m].y].ChangeTileData(new object[2] { null, true });

                if (m_currentPieceTiles[m].x < BoardConsts.REAL_ROWS - 1)
                    _shouldSpawnNewPiece = true;

                filledRows = CheckIfAnyRowIsFilled();
            }
            m_currentPieceTiles = new List<Vector2Int>();

            if (!_shouldSpawnNewPiece)
            {
                Debug.Log("Endgame");
                Debug.Break();
            }
            return filledRows;
        }

        private List<int> CheckIfAnyRowIsFilled()
        {
            List<int> filledRows = new List<int>();
            bool currentRowFilled = true;
            for (int i = 0; i < BoardConsts.REAL_ROWS; i++)
            {
                currentRowFilled = true;
                for (int j = 0; j < BoardConsts.COLUMNS; j++)
                {
                    if (!((TetrisTileData)m_boardController._board[i, j].m_tileData).IsFilled)
                        currentRowFilled = false;
                }

                if (currentRowFilled)
                    filledRows.Add(i);
            }
            return filledRows;
        }

        #endregion Interaction with other pieces

        #region Movement of current Piece

        public void DropPieceTile()
        {
            MovePiecesInSomeDirection(-1, 0);
        }

        public void Change4x4CubeStartTile(int x, int y)
        {
            m_piece4x4CubeStartTile += new Vector2Int(x, y);
        }

        public bool MovePiecesInSomeDirection(int offsetX, int offsetY)
        {
            // All this method O(4*3) = O(12) because the currentPiecesTiles can only be an array of length 4.
            // Constant O

            Vector2Int[] tempCurrentPiecesTiles = new Vector2Int[m_currentPieceTiles.Count];
            m_currentPieceTiles.CopyTo(tempCurrentPiecesTiles);

            //Check if the piece can move
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
                if (!(tempCurrentPiecesTiles[k].x + offsetX >= 0 && tempCurrentPiecesTiles[k].x + offsetX < BoardConsts.TOTAL_ROWS &&
                    tempCurrentPiecesTiles[k].y + offsetY >= 0 && tempCurrentPiecesTiles[k].y + offsetY < BoardConsts.COLUMNS &&
                    !((TetrisTileData)m_boardController._board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY].m_tileData).IsFilled))
                    return false;

            //Reset the previours currentTiles
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
                m_boardController._board[tempCurrentPiecesTiles[k].x, tempCurrentPiecesTiles[k].y].ResetTile();

            //Set new currentTile piece
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
            {
                m_boardController._board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY].ChangeTileData(new object[2] { m_currentPiece.pieceColor, null });
                m_currentPieceTiles[k] = new Vector2Int(tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY);
            }
            Change4x4CubeStartTile(offsetX, offsetY);
            return true;
        }

        public void HardDropPiece(Action callback)
        {
            if (m_currentPieceTiles.Count != 4)
                return;
            // All this method O(4*2 + 4*(_realRows-<highest current piece tile>) ~= O(8+4*(realRows/2)) because the currentPiecesTiles can only be an array of length 4.
            // SemiConstant O

            Vector2Int[] tempCurrentPiecesTiles = new Vector2Int[m_currentPieceTiles.Count];
            m_currentPieceTiles.CopyTo(tempCurrentPiecesTiles);

            CheckLowestNotFilledTile(tempCurrentPiecesTiles, (lowestNotFilledTile, lowestRowInCurrentPiece) =>
             {
                 //Reset the previours currentTiles
                 for (int j = 0; j < tempCurrentPiecesTiles.Length; j++)
                     m_boardController._board[tempCurrentPiecesTiles[j].x, tempCurrentPiecesTiles[j].y].ResetTile();

             // Set Piece to botton
             for (int j = 0; j < tempCurrentPiecesTiles.Length; j++)
             {
                 m_boardController._board[lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece), tempCurrentPiecesTiles[j].y].ChangeTileData(new object[2] { m_currentPiece.pieceColor, null });
                     m_currentPieceTiles[j] = new Vector2Int(lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece), tempCurrentPiecesTiles[j].y);
                 }
                 Change4x4CubeStartTile(3, 0);
                 callback?.Invoke();
             });
        }

        public void RotatePiece(bool clockwise)
        {
            if (clockwise)
            {
                m_currentPieceFormIndex++;
                if (m_currentPieceFormIndex == 4)
                    m_currentPieceFormIndex = 0;
            }
            else
            {
                m_currentPieceFormIndex--;
                if (m_currentPieceFormIndex == -1)
                    m_currentPieceFormIndex = 3;
            }

            m_currentPieceTiles = new List<Vector2Int>();
            ExecuteRotation();
            PaintCurrentPieces();
        }

        private void PaintCurrentPieces()
        {
            for (int j = 0; j < m_currentPieceTiles.Count; j++)
                m_boardController._board[m_currentPieceTiles[j].x, m_currentPieceTiles[j].y].ChangeTileData(new object[2] { m_currentPiece.pieceColor, null });
        }

        private void ExecuteRotation()
        {
            Reset4x4Cube();
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    int tileRow = i + m_piece4x4CubeStartTile.x;
                    int tileColumn = j + m_piece4x4CubeStartTile.y;
                    if (m_currentPiece.pieceForms[m_currentPieceFormIndex].pieceTiles[(3 - i) + (j * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        if (!CheckIfMovementIsOnSideLimits(tileRow, tileColumn))
                        {
                            ResetPieceBeforRotation();
                            return;
                        }

                        if (((TetrisTileData)m_boardController._board[tileRow, tileColumn].m_tileData).IsFilled)
                        {
                            //If any of the tiles collide with a filled tile after the rotation, put the piece one row above the current.
                            m_piece4x4CubeStartTile += new Vector2Int(1, 0);
                            ResetPieceBeforRotation();
                            return;
                        }
                        m_currentPieceTiles.Add(new Vector2Int(tileRow, tileColumn));
                    }
                }
            }
        }

        /// <summary>
        /// Reset the piece befor executing the rotation again
        /// </summary>
        private void ResetPieceBeforRotation()
        {
            m_currentPieceTiles = new List<Vector2Int>();
            Reset4x4Cube();
            ExecuteRotation();
        }

        #endregion Movement of current Piece

        #region Helpers

        public void ClearCurrentPieceTiles(Vector2Int[] tileList, List<Vector2Int> collidingTileList = null)
        {
            for (int j = 0; j < 4; j++)
            {
                if (!((TetrisTileData)m_boardController._board[tileList[j].x, tileList[j].y].m_tileData).IsFilled)
                {
                    //If list of collidingPriority tiles exist the check if any of the tile list collides with this list
                    if (collidingTileList != null)
                    {
                        if (!collidingTileList.Contains(tileList[j]))
                        {
                            m_boardController._board[tileList[j].x, tileList[j].y].ResetTile();
                        }
                    }
                    //If it doesn't exist then just reset it right away
                    else
                    {
                        m_boardController._board[tileList[j].x, tileList[j].y].ResetTile();
                    }
                }
            }
        }

        public bool CheckIfPieceIsInFinalPosition()
        {
            bool pieceFinalPosition = false;
            for (int k = m_currentPieceTiles.Count - 1; k >= 0; k--)
                if (m_currentPieceTiles[k].x == 0 || ((TetrisTileData)m_boardController._board[m_currentPieceTiles[k].x - 1, m_currentPieceTiles[k].y].m_tileData).IsFilled)
                    pieceFinalPosition = true;
            return pieceFinalPosition;
        }

        private void CheckLowestNotFilledTile(Vector2Int[] _currenPieceTiles, Action<int, int> callback)
        {
            int lowestNotFilledTile = 0;
            int lowestRowInCurrentPiece = -1;
            // Found piece's lowest not filled tile.
            for (int k = _currenPieceTiles.Length - 1; k >= 0; k--)
            {
                for (int j = _currenPieceTiles[k].x - 1; j >= 0; j--)
                {
                    if (((TetrisTileData)m_boardController._board[j, _currenPieceTiles[k].y].m_tileData).IsFilled && j + 1 >= lowestNotFilledTile)
                    {
                        if ((j + 1 > lowestNotFilledTile || _currenPieceTiles[k].x <= lowestRowInCurrentPiece || lowestRowInCurrentPiece == -1))
                        {
                            //UGLY UGLY FIX THIS FOR THE LOVE OF GOOD OL GOD
                            bool doesAllTilesInCurrentPieceFitLowest = true;
                            for (int k2 = _currenPieceTiles.Length - 1; k2 >= 0; k2--)
                            {
                                int nextPositionOfTile = (j + 1) + (_currenPieceTiles[k2].x - _currenPieceTiles[k].x);
                                if (nextPositionOfTile < 0 || ((TetrisTileData)m_boardController._board[nextPositionOfTile, _currenPieceTiles[k2].y].m_tileData).IsFilled)
                                {
                                    doesAllTilesInCurrentPieceFitLowest = false;
                                    break;
                                }
                            }
                            if (!doesAllTilesInCurrentPieceFitLowest)
                                break;
                            //END OF UGLU PD: FIX THIIIS!!

                            lowestRowInCurrentPiece = _currenPieceTiles[k].x;
                            lowestNotFilledTile = j + 1;
                            break;
                        }
                    }
                }
            }

            if (lowestRowInCurrentPiece == -1)
            {
                lowestRowInCurrentPiece = BoardConsts.TOTAL_ROWS;
                foreach (Vector2Int currentPieceTile in _currenPieceTiles)
                    if (currentPieceTile.x < lowestRowInCurrentPiece)
                        lowestRowInCurrentPiece = currentPieceTile.x;
            }
            callback?.Invoke(lowestNotFilledTile, lowestRowInCurrentPiece);
        }

        private void Reset4x4Cube()
        {
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    int tileRow = i + m_piece4x4CubeStartTile.x;
                    int tileColumn = j + m_piece4x4CubeStartTile.y;
                    if (tileRow >= 0 && tileRow < BoardConsts.TOTAL_ROWS && tileColumn >= 0 && tileColumn < BoardConsts.COLUMNS)
                    {
                        if (!((TetrisTileData)m_boardController._board[tileRow, tileColumn].m_tileData).IsFilled)
                            m_boardController._board[tileRow, tileColumn].ResetTile();
                    }
                }
            }
        }

        private bool CheckIfMovementIsOnSideLimits(int tileRow, int tileColumn)
        {
            bool noOffsetApplied = true;
            if (tileRow < 0)
            {
                m_piece4x4CubeStartTile.x++;
                noOffsetApplied = false;
            }
            else if (tileRow > BoardConsts.TOTAL_ROWS - 1)
            {
                m_piece4x4CubeStartTile.x--;
                noOffsetApplied = false;
            }

            if (tileColumn < 0)
            {
                m_piece4x4CubeStartTile.y++;
                noOffsetApplied = false;
            }
            else if (tileColumn > BoardConsts.COLUMNS - 1)
            {
                m_piece4x4CubeStartTile.y--;
                noOffsetApplied = false;
            }

            return noOffsetApplied;
        }

        #endregion Helpers

        #endregion Methods
    }
}