using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JiufenGames.TetrisAlike.Logic
{
    public class NextPieceController : BoardTetrisControllerBase
    {
        #region Fields

        //Data
        private Queue<Piece> m_queueofNewPieces = new Queue<Piece>();

        //References
        [SerializeField] private PiecesScriptable m_piecesTypes;

        #endregion Fields

        #region Methods

        public override void Init()
        {
            m_queueofNewPieces.Enqueue(m_piecesTypes.pieces[Random.Range(0, m_piecesTypes.pieces.Length)]);
            CreateBoard(4, 4, BoardConsts.OFFSET_TILES_NEXT_PIECE_BOARD);
        }

        public override void CreateBoard(int _rows, int _columns, float _offsetTiles = 1, Action<int, int> _createdTile = null)
        {
            base.CreateBoard(_rows, _columns, _offsetTiles, _createdTile);
            _createdTile += (row, column) =>
            {
                m_board[row, column].m_tileRow = row;
                m_board[row, column].m_tileColumn = column;
            };
        }

        public void ShowNextPiece()
        {
            Piece nextPiece = m_queueofNewPieces.Peek();
            SpawnPiece(4, 4, nextPiece);
        }

        public Piece GetNextPiece()
        {
            AddNewRandomPieceToQueue();
            return m_queueofNewPieces.Dequeue();
        }

        private void AddNewRandomPieceToQueue()
        {
            m_queueofNewPieces.Enqueue(m_piecesTypes.pieces[Random.Range(0, m_piecesTypes.pieces.Length)]);
        }

        #endregion Methods
    }
}