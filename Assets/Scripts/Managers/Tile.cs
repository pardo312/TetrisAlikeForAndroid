using System;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class Tile : MonoBehaviour
    {
        public int _tileRow;
        public int _tileColumn;

        public bool _isFilled;
        public bool _isPartFromCurrentPiece;

        private SpriteRenderer _spriteRenderer;

        [HideInInspector] public Color _color;
        public bool _isPartOfHiddenBoard = false;

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _color = new Color(0.1f, 0.1f, 0.1f, 1);
        }
        public void ChangeColorOfTile(Color newColor)
        {
            if (!_isPartOfHiddenBoard)
            {
                _color = newColor; 
                _spriteRenderer.color = _color;
            }
        }
        public void Reset()
        {
            if (!_isPartOfHiddenBoard)
            {
                _color = new Color(0.1f, 0.1f, 0.1f, 1);
                _spriteRenderer.color = _color;
            }

            _isFilled = false;
            _isPartFromCurrentPiece = false;
        }

        public void SetPieceToBeHidden()
        {
            ChangeColorOfTile(Color.clear);
            _isPartOfHiddenBoard = true;
        }
    }
}