using JiufenGames.TetrisAlike.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class BoardController : MonoBehaviour
    {
        #region Variables
        [Header("Necessary References")]
        [SerializeField] private Tile _tilePrefab;
        [SerializeField] private PiecesScriptable _piecesTypes;
        [SerializeField] private Transform _tileParent;

        [Header("Board Settings")]
        [SerializeField] private float _offsetTiles = 0.5f;
        [SerializeField] private int _columns = 10;
        [SerializeField] private int _realRows = 20;
        private int _totalRows;

        [HideInInspector] public Tile[,] _board;
        [HideInInspector] public Piece _currentPiece;
        [HideInInspector] public bool _shouldSpawnNewPiece = true;
        private Queue<Piece> _listOfNextPieces = new Queue<Piece>();

        //Gameplay
        [HideInInspector] public List<Vector2Int> _currentPieceTiles;
        private int _currentPieceFormIndex = 0;
        [HideInInspector] public bool userExecutingAction = false;
        [SerializeField, Range(0, 20)] public float _timeBetweenFalls = 0.01f;
        private float _timer = 20;
        private Vector2Int _piece4x4CubeStartTile;

        #endregion

        #region Init
        void Start()
        {
            //3 EmptyRows To handle final round pieces.
            _totalRows = _realRows + 4;
            _board = new Tile[_totalRows, _columns];
            for (int i = 0; i < _totalRows; i++)
                for (int j = 0; j < _columns; j++)
                {
                    _board[i, j] = Instantiate(_tilePrefab, new Vector3(j * (1 + _offsetTiles), i * (1 + _offsetTiles), 0), Quaternion.identity, _tileParent);
                    _board[i, j]._tileRow = i;
                    _board[i, j]._tileColumn = j;
                    if (i == _realRows)
                        _board[i, j].SetFirstHiddenRowPiece();
                    if (i > _realRows)
                        _board[i, j].SetPieceToBeHidden();
                }

            _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
            //_listOfNextPieces.Enqueue(_piecesTypes.pieces[5]);

        }

        #endregion

        #region GameFlow

        #region Update
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _timeBetweenFalls)
                return;
            if (userExecutingAction)
                return;

            _timer = 0;

            if (_shouldSpawnNewPiece)
            {
                _currentPiece = _listOfNextPieces.Dequeue();
                _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
                //_listOfNextPieces.Enqueue(_piecesTypes.pieces[5]);
                SpawnPiece();
                _shouldSpawnNewPiece = false;
                return;
            }


            if (CheckIfPieceIsInFinalPosition())
            {
                CheckTileBelow();
                _timer = _timeBetweenFalls;
                return;
            }

            DropPieceTile();
        }


        #endregion

        #region Spawn
        private void SpawnPiece()
        {
            int offset = 0;
            int highestOffset = 0;
            _currentPieceFormIndex = 0;
            //Spawn Piece in the upper 4x4 space of the board
            for (int i = _realRows - 4; i < _realRows; i++)
            {
                bool rowFilled = false;
                for (int j = 3; j <= 6; j++)
                    if (_board[i, j]._isFilled)
                        rowFilled = true;

                if (rowFilled)
                    offset++;
            }

            _piece4x4CubeStartTile = new Vector2Int(_realRows - 4, 3);
            for (int i = _realRows - 4; i < _realRows; i++)
            {
                for (int j = 3; j <= 6; j++)
                    if (_currentPiece.pieceForms[_currentPieceFormIndex].pieceTiles[((_realRows - 1) - i) + ((j - 3) * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        if (offset > highestOffset)  
                            _piece4x4CubeStartTile = new Vector2Int(i+offset - 4, 3);

                        _board[i + offset, j].ChangeColorOfTile(_currentPiece.pieceColor);
                        _currentPieceTiles.Add(new Vector2Int(i + offset, j));
                    }
            }
        }
        #endregion

        #region Verify current Piece

        private bool CheckIfPieceIsInFinalPosition()
        {
            bool pieceFinalPosition = false;
            for (int k = _currentPieceTiles.Count - 1; k >= 0; k--)
                if (_currentPieceTiles[k].x == 0 || _board[_currentPieceTiles[k].x - 1, _currentPieceTiles[k].y]._isFilled)
                {
                    pieceFinalPosition = true;
                    //CheckIfRowIsFilled(_currentPieceTiles[k].x);
                }
            return pieceFinalPosition;
        }

        private void CheckTileBelow()
        {
            userExecutingAction = true;
            for (int m = _currentPieceTiles.Count - 1; m >= 0; m--)
            {
                _board[_currentPieceTiles[m].x, _currentPieceTiles[m].y]._isFilled = true;

                if (_currentPieceTiles[m].x < _realRows - 2)
                {
                    _shouldSpawnNewPiece = true;
                }
            }
            _currentPieceTiles = new List<Vector2Int>();

            if (!_shouldSpawnNewPiece)
            {
                Debug.Log("Endgame");
                Debug.Break();
            }
            userExecutingAction = false;
        }

        private void CheckIfRowIsFilled(int row)
        {
            for(int i = 0; i< _columns; i++)
                if (!_board[row, i]._isFilled)
                    return;
        }

        #endregion

        #region Move Piece

        private void DropPieceTile()
        {
            MovePiecesInSomeDirection(-1, 0);
        }
        private void Change4x4CubeStartTile(int x, int y)
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
                if (!(tempCurrentPiecesTiles[k].x + offsetX >= 0 && tempCurrentPiecesTiles[k].x + offsetX < _totalRows &&
                    tempCurrentPiecesTiles[k].y + offsetY >= 0 && tempCurrentPiecesTiles[k].y + offsetY < _columns &&
                    !_board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY]._isFilled))
                    return false;

            //Reset the previours currentTiles 
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
                _board[tempCurrentPiecesTiles[k].x, tempCurrentPiecesTiles[k].y].Reset();

            //Set new currentTile piece
            for (int k = 0; k < tempCurrentPiecesTiles.Length; k++)
            {
                _board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY].ChangeColorOfTile(_currentPiece.pieceColor);
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
                    if (_board[k, tempCurrentPiecesTiles[j].y]._isFilled && k + 1 >= lowestNotFilledTile)
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
                lowestRowInCurrentPiece = _totalRows;
                foreach (Vector2Int currentPieceTile in tempCurrentPiecesTiles)
                    if (currentPieceTile.x < lowestRowInCurrentPiece)
                        lowestRowInCurrentPiece = currentPieceTile.x;
            }

            //Reset the previours currentTiles 
            for (int j = 0; j < tempCurrentPiecesTiles.Length; j++)
                _board[tempCurrentPiecesTiles[j].x, tempCurrentPiecesTiles[j].y].Reset();

            // Set Piece to botton
            for (int j = 0; j < tempCurrentPiecesTiles.Length; j++)
            {
                _board[lowestNotFilledTile + (tempCurrentPiecesTiles[j].x - lowestRowInCurrentPiece), tempCurrentPiecesTiles[j].y].ChangeColorOfTile(_currentPiece.pieceColor);
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
                            _currentPieceTiles = new List<Vector2Int>();
                            Reset4x4Cube();
                            ExecuteRotation();
                            return;
                        }

                        if (!_board[tileRow, tileColumn]._isFilled)
                        {
                            if (tileRow > _realRows - 1)
                            {
                                int gb = 0;
                                gb++;
                            }
                            _board[tileRow, tileColumn].ChangeColorOfTile(_currentPiece.pieceColor);
                            _currentPieceTiles.Add(new Vector2Int(tileRow, tileColumn));
                        }
                    }
                }
            }
        }
        private void Reset4x4Cube()
        {
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {

                    int tileRow = i + _piece4x4CubeStartTile.x;
                    int tileColumn = j + _piece4x4CubeStartTile.y;
                    if (tileRow >= 0 && tileRow < _totalRows && tileColumn >= 0 && tileColumn < _columns)
                    {
                        if (!_board[tileRow, tileColumn]._isFilled)
                            _board[tileRow, tileColumn].Reset();
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
            else if (tileRow > _totalRows - 1)
            {
                _piece4x4CubeStartTile.x--;
                noOffsetApplied = false;
            }

            if (tileColumn < 0)
            {
                _piece4x4CubeStartTile.y++;
                noOffsetApplied = false;
            }
            else if (tileColumn > _columns - 1)
            {
                _piece4x4CubeStartTile.y--;
                noOffsetApplied = false;
            }

            return noOffsetApplied;
        }
        #endregion

        #endregion
    }
}
