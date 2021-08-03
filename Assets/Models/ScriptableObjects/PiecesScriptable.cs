
using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{
    [CreateAssetMenu(fileName = "PiecesScriptable.asset", menuName = "TetrisAlike/PiecesScriptable")]
    public class PiecesScriptable : ScriptableObject
    {
        public Piece[] pieces = new Piece[1];
    }
}