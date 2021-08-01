using UnityEditor;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{
    [System.Serializable]
    public class PieceForm
    {
        public string pieceFormName = "Need Name";
        //Indicate the form of the piece
        public bool[,] PieceTiles = new bool[4,4];
    }
}