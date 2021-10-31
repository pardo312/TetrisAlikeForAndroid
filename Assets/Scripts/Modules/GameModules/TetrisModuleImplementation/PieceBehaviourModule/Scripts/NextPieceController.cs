using JiufenGames.TetrisAlike.Model;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class NextPieceController : MonoBehaviour
    {
        #region Fields

        //Data
        public HideableTileBase[,] m_4x4board;

        private Queue<Piece> m_queueofNewPieces = new Queue<Piece>();

        //References
        [SerializeField] private PiecesScriptable m_piecesTypes;

        [SerializeField] private Transform m_nextPieceTileParent;
        [SerializeField] private HideableTileBase m_tilePrefab;

        #endregion Fields

        #region Methods

        public void Init()
        {
            m_queueofNewPieces.Enqueue(m_piecesTypes.pieces[Random.Range(0, m_piecesTypes.pieces.Length)]);
            Init4x4NextPieceBoard();
        }

        private void Init4x4NextPieceBoard()
        {
            m_4x4board = new HideableTileBase[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_4x4board[i, j] = Instantiate(m_tilePrefab, m_nextPieceTileParent.position + new Vector3(j * (1 * BoardConsts.OFFSET_TILES_NEXT_PIECE_BOARD), i * (1 * BoardConsts.OFFSET_TILES_NEXT_PIECE_BOARD), 0), Quaternion.identity, m_nextPieceTileParent);
                    m_4x4board[i, j].transform.localScale = m_nextPieceTileParent.localScale;
                    m_4x4board[i, j].m_tileRow = i;
                    m_4x4board[i, j].m_tileColumn = j;
                }
            }
        }

        public void ShowNextPiece()
        {
            Piece nextPiece = m_queueofNewPieces.Peek();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (nextPiece.pieceForms[0].pieceTiles[(3 - i) + ((j) * PieceForm.PIECE_TILES_WIDTH)])
                        m_4x4board[i, j].ChangeTileData(new object[2] { nextPiece.pieceColor, null });
                    else
                        m_4x4board[i, j].ChangeTileData(new object[2] { PieceConsts.DEFAULT_COLOR, null });
                }
            }
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