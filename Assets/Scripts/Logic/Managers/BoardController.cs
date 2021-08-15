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
        private bool _shouldSpawnNewPiece = true;
        private Queue<Piece> _listOfNextPieces = new Queue<Piece>();

        //Gameplay
        [HideInInspector] public List<Vector2Int> _currentPieceTiles;
        [HideInInspector] public bool userExecutingAction = false;
        [SerializeField, Range(0, 20)] public float _timeBetweenFalls = 0.01f;
        private float _timer = 20;

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

            for (int k = 0; k < _piecesTypes.pieces.Length - 1; k++)
                _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
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
                SpawnPiece();
                _shouldSpawnNewPiece = false;
                return;
            }


            if (CheckIfPieceIsInFinalPosition())
            {
                CheckTileBelow();
                return;
            }

            DropPieceTile();
        }
        #endregion

        #region Spawn
        private void SpawnPiece()
        {
            int offset = 0;
            //Spawn Piece in the upper 4x4 space of the board
            for (int i = _realRows - 4; i < _realRows; i++)
            {
                bool rowFilled = false;
                for (int j = 3; j <= 6; j++)
                    if (_board[i, j]._isFilled)
                        rowFilled = true;

                if(rowFilled)
                    offset++;
            }
            for (int i = _realRows - 4; i < _realRows; i++)
            {
                for (int j = 3; j <= 6; j++)
                    if (_currentPiece.pieceForms[0].pieceTiles[((_realRows - 1) - i) + ((j - 3) * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        _board[i + offset, j].ChangeColorOfTile(_currentPiece.pieceColor);
                        _currentPieceTiles.Add(new Vector2Int(i+offset, j));
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
                    pieceFinalPosition = true;
            return pieceFinalPosition;
        }

        private void CheckTileBelow()
        {
            for (int m = _currentPieceTiles.Count - 1; m >= 0; m--)
            {
                _board[_currentPieceTiles[m].x, _currentPieceTiles[m].y]._isFilled = true;

                if (_currentPieceTiles[m].x < _realRows-2)
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
        }
        #endregion

        #region Move Piece

        private void DropPieceTile()
        {
            MovePiecesInSomeDirection(-1, 0);
        }

        public bool MovePiecesInSomeDirection(int offsetX,int offsetY)
        {
            // All this method O(4*3) = O(12) because the currentPiecesTiles can only be an array of length 4.

            Vector2Int[] tempCurrentPiecesTiles = new Vector2Int[_currentPieceTiles.Count];
            _currentPieceTiles.CopyTo(tempCurrentPiecesTiles);

            //Check if the piece can move
            for (int k = 0; k < tempCurrentPiecesTiles.Length ; k++)
                if (!(tempCurrentPiecesTiles[k].x + offsetX >= 0 && tempCurrentPiecesTiles[k].y + offsetY < _totalRows &&
                    tempCurrentPiecesTiles[k].y + offsetY >= 0 && tempCurrentPiecesTiles[k].y + offsetY < _columns &&
                    !_board[tempCurrentPiecesTiles[k].x + offsetX,tempCurrentPiecesTiles[k].y + offsetY]._isFilled))
                        return false;

            //Reset the previours currentTiles 
            for (int k = 0; k < tempCurrentPiecesTiles.Length ; k++)
                _board[tempCurrentPiecesTiles[k].x, tempCurrentPiecesTiles[k].y].Reset();

            //Set new currentTile piece
            for (int k = 0; k < tempCurrentPiecesTiles.Length ; k++)
            {
                _board[tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY].ChangeColorOfTile(_currentPiece.pieceColor);
                _currentPieceTiles[k] = new Vector2Int(tempCurrentPiecesTiles[k].x + offsetX, tempCurrentPiecesTiles[k].y + offsetY);
            }
            return true;
        }
        #endregion

        #endregion
    }
}
