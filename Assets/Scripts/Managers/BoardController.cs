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
                    _board[i, j] = Instantiate(_tilePrefab, new Vector3(j*(1+_offsetTiles),i*(1+_offsetTiles), 0), Quaternion.identity,_tileParent);

            for (int k = 0; k < _piecesTypes.pieces.Length - 1; k++)
                _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
        }

        void Update()
        {
            if (!_shouldSpawnNewPiece)
                return;
            if (_currentPiece == null)
            {
                _currentPiece = _listOfNextPieces.Dequeue();
                _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);

                

            }
            else
            {

            }


        }
    }
}
