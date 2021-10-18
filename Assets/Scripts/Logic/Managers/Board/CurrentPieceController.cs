using JiufenGames.TetrisAlike.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class CurrentPieceController
    {
        public Piece _currentPiece;
        [HideInInspector] public List<Vector2Int> _currentPieceTiles;
        public Vector2Int _piece4x4CubeStartTile;
        private int _currentPieceFormIndex = 0;

        #region References
        private BoardController _boardController;
        #endregion References

        #region Methods
        public void Init(BoardController boardController)
        {
            _boardController = boardController;
        }
        //TEST
        private int pieceNumber = 0;

        //EndTEST
        public void OnSpawn()
        {
            pieceNumber++;
            _currentPieceFormIndex = 0;
        }
        public bool CheckIfPieceIsInFinalPosition()
        {
            bool pieceFinalPosition = false;
            for (int k = _currentPieceTiles.Count - 1; k >= 0; k--)
                if (_currentPieceTiles[k].x == 0 || _boardController._board[_currentPieceTiles[k].x - 1, _currentPieceTiles[k].y]._isFilled)
                {
                    pieceFinalPosition = true;
                    //CheckIfRowIsFilled(_currentPieceTiles[k].x);
                }
            return pieceFinalPosition;
        }



        /// <summary>
        /// When the current piece drops to the lowest possition, check if the tile bellow is inside te tetris board. If not end the game
        /// </summary>
        /// <param name="_shouldSpawnNewPiece"></param>
        /// <returns>Filled rows</returns>
        public List<int> CheckTileBelow(ref bool _shouldSpawnNewPiece)
        {
            List<int> filledRows = new List<int>();
            for (int m = _currentPieceTiles.Count - 1; m >= 0; m--)
            {
                _boardController._board[_currentPieceTiles[m].x, _currentPieceTiles[m].y]._isFilled = true;

                if (_currentPieceTiles[m].x < Consts.REAL_ROWS - 2)
                    _shouldSpawnNewPiece = true;

                filledRows = CheckIfAnyRowIsFilled();
            }
            _currentPieceTiles = new List<Vector2Int>();

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
            for (int i = 0; i < Consts.REAL_ROWS; i++)
            {
                currentRowFilled = true;
                for (int j = 0; j < Consts.COLUMNS; j++)
                {
                    if (!_boardController._board[i, j]._isFilled)
                        currentRowFilled = false;
                }

                if (currentRowFilled)
                    filledRows.Add(i);
            }
            return filledRows;
        }

        public void DropPieceTile()
        {
            MovePiecesInSomeDirection(-1, 0);
        }
        public void Change4x4CubeStartTile(int x, int y)
        {
            _piece4x4CubeStartTile += new Vector2Int(x, y);
        }


        public bool MovePiecesInSomeDirection(int offsetX, int offsetY)
        {
            // All this method O(4*3) = O(12) because the currentPiecesTiles can only be an array of length 4.
            // Constant O

            Vector2Int[] tempCurrentPiecesTiles = new Vector2Int[_currentPieceTiles.Count];
            _currentPieceTiles.CopyTo(tempCurrentPiecesTiles);

            //Check if the piece can move
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
                if (!(tempCurrentPiecesTiles[k].x + offsetX >= 0 && tempCurrentPiecesTiles[k].x + offsetX < Consts.TOTAL_ROWS &&
                    tempCurrentPiecesTiles[k].y + offsetY >= 0 && tempCurrentPiecesTiles[k].y + offsetY < Consts.COLUMNS &&
                    !_boardController._board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY]._isFilled))
                    return false;

            //Reset the previours currentTiles 
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
                _boardController._board[tempCurrentPiecesTiles[k].x, tempCurrentPiecesTiles[k].y].Reset();

            //Set new currentTile piece
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
            {
                _boardController._board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY].ChangeColorOfTile(_currentPiece.pieceColor);
                _currentPieceTiles[k] = new Vector2Int(tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY);
            }
            Change4x4CubeStartTile(offsetX, offsetY);
            return true;
        }
        public bool HardDropPiece()
        {
            if (_currentPieceTiles.Count != 4)
                return false;
            // All this method O(4*2 + 4*(_realRows-<highest current piece tile>) ~= O(8+4*(realRows/2)) because the currentPiecesTiles can only be an array of length 4.
            // SemiConstant O

            Vector2Int[] tempCurrentPiecesTiles = new Vector2Int[_currentPieceTiles.Count];
            _currentPieceTiles.CopyTo(tempCurrentPiecesTiles);


            int lowestNotFilledTile = 0;
            int lowestRowInCurrentPiece = -1;
            // Found piece's lowest not filled tile.
            for (int j = tempCurrentPiecesTiles.Length - 1; j >= 0; j--)
            {
                for (int k = tempCurrentPiecesTiles[j].x - 1; k >= 0; k--)
                {
                    if (_boardController._board[k, tempCurrentPiecesTiles[j].y]._isFilled && k + 1 >= lowestNotFilledTile)
                    {
                        if ((k + 1 > lowestNotFilledTile || tempCurrentPiecesTiles[j].x < lowestRowInCurrentPiece || lowestRowInCurrentPiece == -1))
                            lowestRowInCurrentPiece = tempCurrentPiecesTiles[j].x;

                        lowestNotFilledTile = k + 1;
                        break;
                    }
                }
            }


            if (lowestRowInCurrentPiece == -1)
            {
                lowestRowInCurrentPiece = Consts.TOTAL_ROWS;
                foreach (Vector2Int currentPieceTile in tempCurrentPiecesTiles)
                    if (currentPieceTile.x < lowestRowInCurrentPiece)
                        lowestRowInCurrentPiece = currentPieceTile.x;
            }

            //Reset the previours currentTiles 
            for (int j = 0; j < tempCurrentPiecesTiles.Length; j++)
                _boardController._board[tempCurrentPiecesTiles[j].x, tempCurrentPiecesTiles[j].y].Reset();

            // Set Piece to botton
            for (int j = 0; j < tempCurrentPiecesTiles.Length; j++)
            {
                _boardController._board[lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece), tempCurrentPiecesTiles[j].y].ChangeColorOfTile(_currentPiece.pieceColor);
                _currentPieceTiles[j] = new Vector2Int(lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece), tempCurrentPiecesTiles[j].y);
            }
            Change4x4CubeStartTile(3, 0);
            return true;
        }

        public void RotatePiece(bool clockwise)
        {
            if (clockwise)
            {
                _currentPieceFormIndex++;
                if (_currentPieceFormIndex == 4)
                    _currentPieceFormIndex = 0;
            }
            else
            {
                _currentPieceFormIndex--;
                if (_currentPieceFormIndex == -1)
                    _currentPieceFormIndex = 3;

            }

            _currentPieceTiles = new List<Vector2Int>();
            ExecuteRotation();
            PaintCurrentPieces();
        }

        private void PaintCurrentPieces()
        {
            for (int j = 0; j < _currentPieceTiles.Count; j++)
                _boardController._board[_currentPieceTiles[j].x, _currentPieceTiles[j].y].ChangeColorOfTile(_currentPiece.pieceColor);
        }

        private void ExecuteRotation()
        {
            Reset4x4Cube();
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    int tileRow = i + _piece4x4CubeStartTile.x;
                    int tileColumn = j + _piece4x4CubeStartTile.y;
                    if (_currentPiece.pieceForms[_currentPieceFormIndex].pieceTiles[(3 - i) + (j * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        if (!CheckIfMovementIsOnSideLimits(tileRow, tileColumn))
                        {
                            ResetPieceBeforRotation();
                            return;
                        }

                        if (_boardController._board[tileRow, tileColumn]._isFilled)
                        {
                            //If any of the tiles collide with a filled tile after the rotation, put the piece one row above the current.
                            _piece4x4CubeStartTile += new Vector2Int(1, 0);
                            ResetPieceBeforRotation();
                            return;
                        }
                        _currentPieceTiles.Add(new Vector2Int(tileRow, tileColumn));
                    }
                }
            }
        }

        /// <summary>
        /// Reset the piece befor executing the rotation again
        /// </summary>
        private void ResetPieceBeforRotation()
        {
            _currentPieceTiles = new List<Vector2Int>();
            Reset4x4Cube();
            ExecuteRotation();
        }

        private void Reset4x4Cube()
        {
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {

                    int tileRow = i + _piece4x4CubeStartTile.x;
                    int tileColumn = j + _piece4x4CubeStartTile.y;
                    if (tileRow >= 0 && tileRow < Consts.TOTAL_ROWS && tileColumn >= 0 && tileColumn < Consts.COLUMNS)
                    {
                        if (!_boardController._board[tileRow, tileColumn]._isFilled)
                            _boardController._board[tileRow, tileColumn].Reset();
                    }
                }
            }

        }
        private bool CheckIfMovementIsOnSideLimits(int tileRow, int tileColumn)
        {
            bool noOffsetApplied = true;
            if (tileRow < 0)
            {
                _piece4x4CubeStartTile.x++;
                noOffsetApplied = false;
            }
            else if (tileRow > Consts.TOTAL_ROWS - 1)
            {
                _piece4x4CubeStartTile.x--;
                noOffsetApplied = false;
            }

            if (tileColumn < 0)
            {
                _piece4x4CubeStartTile.y++;
                noOffsetApplied = false;
            }
            else if (tileColumn > Consts.COLUMNS - 1)
            {
                _piece4x4CubeStartTile.y--;
                noOffsetApplied = false;
            }

            return noOffsetApplied;
        }
    }
    #endregion Methods
}
