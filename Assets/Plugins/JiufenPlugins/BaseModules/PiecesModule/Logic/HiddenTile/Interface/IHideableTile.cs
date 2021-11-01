using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public interface IHideableTile : ITile
    {
        bool m_isPartOfHiddenBoard { get; set; }
        bool m_isPartOfFirstRowAfterRealBoard { get; set; }

        void SetPieceToBeHidden();
        void SetFirstHiddenRowPiece();
    }
}