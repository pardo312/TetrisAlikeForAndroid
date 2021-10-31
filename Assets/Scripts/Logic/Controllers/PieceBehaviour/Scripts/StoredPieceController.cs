using JiufenGames.TetrisAlike.Model;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class StoredPieceController : MonoBehaviour
    {
        #region Fields

        //Data
        public Tile[,] m_4x4board;

        private Queue<Piece> m_queueofStored = new Queue<Piece>();

        //References
        [SerializeField] private Transform m_storePieceTilesParent;

        [SerializeField] private Tile m_tilePrefab;

        #endregion Fields

        #region Methods

        public void Init()
        {
            Init4x4NextPieceBoard();
        }

        private void Init4x4NextPieceBoard()
        {
            m_4x4board = new Tile[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_4x4board[i, j] = Instantiate(m_tilePrefab, m_storePieceTilesParent.position + new Vector3(j * (1 * BoardConsts.OFFSET_TILES_NEXT_PIECE_BOARD), i * (1 * BoardConsts.OFFSET_TILES_NEXT_PIECE_BOARD), 0), Quaternion.identity, m_storePieceTilesParent);
                    m_4x4board[i, j].transform.localScale = m_storePieceTilesParent.localScale;
                    m_4x4board[i, j].m_tileRow = i;
                    m_4x4board[i, j].m_tileColumn = j;
                }
            }
        }

        public Piece StorePiece(Piece piece)
        {
            Piece deStoredPiece = null;
            if (m_queueofStored.Count > 0)
                deStoredPiece = m_queueofStored.Dequeue();

            m_queueofStored.Enqueue(piece);
            ShowStoredPiece(piece);

            return deStoredPiece;
        }

        private void ShowStoredPiece(Piece storedPiece)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (storedPiece.pieceForms[0].pieceTiles[(3 - i) + ((j) * PieceForm.PIECE_TILES_WIDTH)])
                        m_4x4board[i, j].ChangeColorOfTile(storedPiece.pieceColor);
                    else
                        m_4x4board[i, j].ChangeColorOfTile(PieceConsts.DEFAULT_COLOR);
                }
            }
        }

        #endregion Methods
    }
}