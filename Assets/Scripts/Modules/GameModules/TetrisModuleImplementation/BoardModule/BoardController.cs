using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class BoardController : MonoBehaviour
    {
        #region Variables

        [Header("Necessary References")]
        [SerializeField] private HideableTileBase _tilePrefab;

        [SerializeField] private Transform _tileParent;

        [Header("Board")]
        [HideInInspector] public HideableTileBase[,] _board;

        #endregion Variables

        #region Init

        public void Init()
        {
            //3 EmptyRows To handle final round pieces.
            _board = new HideableTileBase[BoardConsts.TOTAL_ROWS, BoardConsts.COLUMNS];
            for (int i = 0; i < BoardConsts.TOTAL_ROWS; i++)
                for (int j = 0; j < BoardConsts.COLUMNS; j++)
                {
                    _board[i, j] = Instantiate(_tilePrefab, _tileParent.position + new Vector3(j * (1 * BoardConsts.OFFSET_TILES), i * (1 * BoardConsts.OFFSET_TILES), 0), Quaternion.identity, _tileParent);
                    _board[i, j].transform.localScale = _tileParent.localScale;
                    _board[i, j].m_tileRow = i;
                    _board[i, j].m_tileColumn = j;
                    if (i == BoardConsts.REAL_ROWS)
                        _board[i, j].SetFirstHiddenRowPiece();
                    if (i > BoardConsts.REAL_ROWS)
                        _board[i, j].SetPieceToBeHidden();
                }
        }

        public void ClearCompletedLine(List<int> filledRows)
        {
            for (int i = filledRows.Count - 1; i >= 0; i--)
            {
                ResetLine(filledRows[i]);
                DropUpperLinesOfCurrentLine(filledRows[i]);
            }
        }

        private void ResetLine(int row)
        {
            for (int i = 0; i < BoardConsts.COLUMNS; i++)
                _board[row, i].ResetTile();
        }

        private void DropUpperLinesOfCurrentLine(int row)
        {
            for (int i = row + 1; i < BoardConsts.REAL_ROWS; i++)
            {
                DropLine(i);
                ResetLine(i);
            }
        }

        private void DropLine(int currentLine)
        {
            for (int j = 0; j < BoardConsts.COLUMNS; j++)
            {
                TetrisTileData tileData = _board[currentLine, j].m_tileData as TetrisTileData;
                _board[currentLine - 1, j].ChangeTileData(new object[2] { tileData.Color, null });

                if (tileData.IsFilled)
                    _board[currentLine - 1, j].ChangeTileData(new object[2] { null, true });
            }
        }

        #endregion Init
    }
}