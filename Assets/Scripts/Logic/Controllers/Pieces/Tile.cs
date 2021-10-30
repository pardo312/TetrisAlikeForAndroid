using System;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class Tile : MonoBehaviour
    {
        public int m_tileRow;
        public int m_tileColumn;

        public bool m_isFilled;

        private SpriteRenderer m_spriteRenderer;

        [HideInInspector] public Color m_color;

        public bool m_isPartOfHiddenBoard = false;
        public bool m_isPartOfFirstRowAfterRealBoard = false;

        public void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_color = BoardConsts.DEFAULT_COLOR;
        }

        public void ChangeColorOfTile(Color newColor, bool changeAlpha = false)
        {
            if (m_tileRow > 20)
            {
                int i = 923;
            }
            if (!changeAlpha)
            {
                Color resetedColor = m_spriteRenderer.color;
                resetedColor.a = 1;
                m_spriteRenderer.color = resetedColor;
            }

            if (((m_isPartOfFirstRowAfterRealBoard && newColor.Equals(BoardConsts.DEFAULT_COLOR)) || m_isPartOfHiddenBoard))
                newColor = Color.clear;
            m_color = newColor;
            m_spriteRenderer.color = m_color;

        }

        public void Reset()
        {
            if (m_isPartOfFirstRowAfterRealBoard)
                ChangeColorOfTile(Color.clear);
            else if (!m_isPartOfHiddenBoard)
                ChangeColorOfTile(BoardConsts.DEFAULT_COLOR);

            m_isFilled = false;
        }

        public void SetPieceToBeHidden()
        {
            m_isPartOfHiddenBoard = true;
            ChangeColorOfTile(Color.clear);
        }

        public void SetFirstHiddenRowPiece()
        {
            m_isPartOfFirstRowAfterRealBoard = true;
            ChangeColorOfTile(Color.clear);
        }
    }
}