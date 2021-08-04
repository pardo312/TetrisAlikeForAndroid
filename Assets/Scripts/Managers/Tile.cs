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

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _color = new Color(0.1f, 0.1f, 0.1f, 1);
        }
        public void ChangeColorOfTile(Color newColor)
        {
            _color = newColor; 
            _spriteRenderer.color = _color;

        }
        public void Reset()
        {
            _color = new Color(0.1f, 0.1f, 0.1f, 1);
            _spriteRenderer.color = _color;
            _isFilled = false;
            _isPartFromCurrentPiece = false;
        }
    }
}