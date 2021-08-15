using System;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class Tile : MonoBehaviour
    {
        public int _tileRow;
        public int _tileColumn;

        public bool _isFilled;

        private SpriteRenderer _spriteRenderer;

        [HideInInspector] public Color _color;

        public bool _isPartOfHiddenBoard = false;
        public bool _isPartOfFirstRowAfterRealBoard = false;

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _color = Consts._defaultColor;
        }

        public void ChangeColorOfTile(Color newColor)
        {
            if (((_isPartOfFirstRowAfterRealBoard || _isPartOfHiddenBoard) && newColor.Equals(Consts._defaultColor)))
                newColor = Color.clear;
            if (_isPartOfHiddenBoard && !newColor.Equals(Color.clear))
                return;
            _color = newColor;
            _spriteRenderer.color = _color;
        }

        public void Reset()
        {
            if (!_isPartOfHiddenBoard || !_isPartOfFirstRowAfterRealBoard)
                ChangeColorOfTile(Consts._defaultColor);

            _isFilled = false;
        }

        public void SetPieceToBeHidden()
        {
            _isPartOfHiddenBoard = true;
            ChangeColorOfTile(Color.clear);
        }

        public void SetFirstHiddenRowPiece()
        {
            _isPartOfFirstRowAfterRealBoard = true;
            ChangeColorOfTile(Color.clear);
        }
    }
}