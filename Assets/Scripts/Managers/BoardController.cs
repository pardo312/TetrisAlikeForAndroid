using JiufenGames.TetrisAlike.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class BoardController : MonoBehaviour
    {
        [Header("Necessary References")]
        [SerializeField] private Tile _tilePrefab;
        [SerializeField] private PiecesScriptable _piecesTypes;
        [SerializeField] private Transform _tileParent;

        [Header("Board Settings")]
        [SerializeField] private int _rows = 12;
        [SerializeField] private int _columns = 6;
        [SerializeField] private float _offsetTiles = 0.5f;


        private Piece _currentPiece;
        private bool _shouldSpawnNewPiece = true;
        private Tile[,] _board;
        private Queue<Piece> _listOfNextPieces = new Queue<Piece>();

        void Start()
        {
            _board = new Tile[_rows, _columns];
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                {
                    _board[i, j] = Instantiate(_tilePrefab, new Vector3(j * (1 + _offsetTiles), i * (1 + _offsetTiles), 0), Quaternion.identity, _tileParent);
                    _board[i, j]._tileRow = i;
                    _board[i, j]._tileColumn = j;
                }

            for (int k = 0; k < _piecesTypes.pieces.Length - 1; k++)
                _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
        }
        private float _timeBetweenFalls = 0.4f;
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
            Vector2Int[] pieceTiles = new Vector2Int[4];
            int currentIndexPieceTiles = 0;

            for (int i = _rows - 1; i > 0; i--)
                for (int j = 0; j < _columns - 1; j++)
                    if (_board[i, j]._isPartFromCurrentPiece)
                    {
                        pieceTiles[currentIndexPieceTiles] = new Vector2Int(i, j);
                        currentIndexPieceTiles++;
                    }

            for (int k = 3; k >= 0; k--)
            {
                if(pieceTiles[k].x-1 == 0 || _board[pieceTiles[k].x - 2, pieceTiles[k].y]._isFilled)
                    _shouldSpawnNewPiece = true;
            }
            for (int k = 3; k >= 0; k--)
            {
                if(_shouldSpawnNewPiece)
                    _board[pieceTiles[k].x - 1, pieceTiles[k].y]._isFilled = true;
                else
                    _board[pieceTiles[k].x - 1, pieceTiles[k].y]._isPartFromCurrentPiece = true;

                _board[pieceTiles[k].x - 1, pieceTiles[k].y].ChangeColorOfTile(_currentPiece.pieceColor);
                _board[pieceTiles[k].x, pieceTiles[k].y].Reset();
            }

        }

        private void SpawnPiece()
        {
            //Spawn Piece in the upper 4x4 space of the board
            for (int i = _rows - 4; i <= _rows - 1; i++)
                for (int j = 3; j <= 6; j++)
                    if (_currentPiece.pieceForms[0].pieceTiles[(_rows - 1 - i) + ((j - 3) * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        _board[i, j].ChangeColorOfTile(_currentPiece.pieceColor);
                        _board[i, j]._isPartFromCurrentPiece = true;
                    }
        }
    }
}
