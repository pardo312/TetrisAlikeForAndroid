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

        private Piece _currentPiece;
        private bool _shouldSpawnNewPiece = true;
        private Tile[,] _board;
        private Queue<Piece> _listOfNextPieces = new Queue<Piece>();
        #endregion

        #region Init
        void Start()
        {
            //3 EmptyRows To handle final round pieces.
            _totalRows = _realRows + 3;
            _board = new Tile[_totalRows, _columns];
            for (int i = 0; i < _totalRows; i++)
                for (int j = 0; j < _columns; j++)
                {
                    _board[i, j] = Instantiate(_tilePrefab, new Vector3(j * (1 + _offsetTiles), i * (1 + _offsetTiles), 0), Quaternion.identity, _tileParent);
                    _board[i, j]._tileRow = i;
                    _board[i, j]._tileColumn = j;
                    if (i >= _realRows)
                    {
                        _board[i, j].SetPieceToBeHidden();
                    }
                }

            for (int k = 0; k < _piecesTypes.pieces.Length - 1; k++)
                _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
        }

        #endregion

        #region GameFlow

        #region Update
        private float _timeBetweenFalls = 0.01f;
        private float _timer = 2;
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _timeBetweenFalls)
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

            List<Vector2Int> pieceTiles;
            SearchForCurrentPieceTiles(out pieceTiles);

            if (CheckIfPieceIsInFinalPosition(pieceTiles))
            {
                CheckTileBelow(pieceTiles);
                return;
            }

            DropPieceTile(pieceTiles);
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
                        _board[i + offset, j]._isPartFromCurrentPiece = true;
                    }
            }
        }
        #endregion

        #region Verify current Piece
        private void SearchForCurrentPieceTiles(out List<Vector2Int> pieceTiles)
        {
            pieceTiles = new List<Vector2Int>();
            //Search For current Pieces Tiles

            for (int i = _totalRows - 1; i >= 0; i--)
            {
                for (int j = 0; j < _columns; j++)
                    if (_board[i, j]._isPartFromCurrentPiece)
                    {
                        pieceTiles.Add(new Vector2Int(i, j));
                    }
            }
        }

        private bool CheckIfPieceIsInFinalPosition(List<Vector2Int> pieceTiles)
        {
            bool pieceFinalPosition = false;
            for (int k = pieceTiles.Count - 1; k >= 0; k--)
                if (pieceTiles[k].x == 0 || _board[pieceTiles[k].x - 1, pieceTiles[k].y]._isFilled)
                    pieceFinalPosition = true;
            return pieceFinalPosition;
        }

        private void CheckTileBelow(List<Vector2Int> pieceTiles)
        {
            for (int m = pieceTiles.Count - 1; m >= 0; m--)
            {
                _board[pieceTiles[m].x, pieceTiles[m].y]._isPartFromCurrentPiece = false;
                _board[pieceTiles[m].x, pieceTiles[m].y]._isFilled = true;

                if (pieceTiles[m].x < _realRows-2)
                {
                    _shouldSpawnNewPiece = true;
                }
            }
            if (!_shouldSpawnNewPiece)
            {
                Debug.Log("Endgame");
                Debug.Break();
            }
        }
        #endregion

        #region Move Piece

        private void DropPieceTile(List<Vector2Int> pieceTiles)
        {
            for (int k = pieceTiles.Count - 1; k >= 0; k--)
            {
                _board[pieceTiles[k].x - 1, pieceTiles[k].y]._isPartFromCurrentPiece = true;
                _board[pieceTiles[k].x - 1, pieceTiles[k].y].ChangeColorOfTile(_currentPiece.pieceColor);
                _board[pieceTiles[k].x, pieceTiles[k].y].Reset();
            }
        }
        #endregion

        #endregion
    }
}
