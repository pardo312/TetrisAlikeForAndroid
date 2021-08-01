
using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{

    [CreateAssetMenu(fileName = "Piece.asset", menuName = "TetrisAlike/Piece")]
    [System.Serializable]
    public class Piece:ScriptableObject 
    {
        public string PieceName;
        public PieceForm[] PieceForms = new PieceForm[4];
        public Color PieceColor = Color.green;
    }
}