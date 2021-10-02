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
        [SerializeField] private Transform _tileParent;

        [Header("Board Settings")]
        public float _offsetTiles = 0.5f;
        public int _columns = 10;
        public int _realRows = 20;
        [HideInInspector]public int _totalRows;


        [Header("Board")]
        [HideInInspector] public Tile[,] _board;


        #endregion

        #region Init
        public void Init()
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

        }

        #endregion
    }
}
