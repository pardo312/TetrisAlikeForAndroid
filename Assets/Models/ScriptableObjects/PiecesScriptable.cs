
using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{
    [CreateAssetMenu(fileName = "PiecesScriptable.asset", menuName = "TetrisAlike/PiecesScriptable")]
    public class PiecesScriptable : ScriptableObject
    {
        public Piece[] Pieces = new Piece[1];
    }
}