using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{
    [CreateAssetMenu(fileName = "PiecesScriptable.asset", menuName = "TetrisAlike/Pieces/PiecesScriptable")]
    public class PiecesScriptable : ScriptableObject
    {
        public Piece[] pieces = new Piece[1];
    }
}