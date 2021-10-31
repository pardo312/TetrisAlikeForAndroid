using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{
    [CreateAssetMenu(fileName = "Piece.asset", menuName = "TetrisAlike/Pieces/Piece")]
    [System.Serializable]
    public class Piece : ScriptableObject
    {
        public string pieceName;
        public PieceForm[] pieceForms = new PieceForm[4];
        public Color pieceColor = Color.green;
    }
}