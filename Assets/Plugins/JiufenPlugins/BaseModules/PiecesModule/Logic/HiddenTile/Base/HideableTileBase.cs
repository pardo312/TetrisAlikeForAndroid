using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{

    public abstract class HideableTileBase : MonoBehaviour, IHideableTile
    {
        #region Fields
        #region Interface 
        #region BackingFields
        private int m_tileRowField;
        private int m_tileColumnField;
        private bool m_isPartOfHiddenBoardField = false;
        private bool m_isPartOfFirstRowAfterRealBoardField = false;
        private object m_tileDataField = null;
        #endregion BackingFields

        #region Properties
        public int m_tileRow { get => m_tileRowField; set => m_tileRowField = value; }
        public int m_tileColumn { get => m_tileColumnField; set => m_tileColumnField = value; }
        public bool m_isPartOfHiddenBoard { get => m_isPartOfHiddenBoardField; set => m_isPartOfHiddenBoardField = value; }
        public bool m_isPartOfFirstRowAfterRealBoard { get => m_isPartOfFirstRowAfterRealBoardField; set => m_isPartOfFirstRowAfterRealBoardField = value; }
        public object m_tileData  { get => m_tileDataField  ; set => m_tileDataField   = value; }
        #endregion Properties
        #endregion Interface 
        #endregion Fields

        #region Methods
        public abstract void Awake();


        public virtual void ResetTile()
        {
            ChangeTileData();
        }

        public virtual void SetPieceToBeHidden()
        {
            m_isPartOfHiddenBoard = true;
            ChangeTileData();
        }

        public virtual void SetFirstHiddenRowPiece()
        {
            m_isPartOfFirstRowAfterRealBoard = true;
            ChangeTileData();
        }

        public virtual object[] ChangeTileData(object[] _methodParams = null)
        {
            if (_methodParams == null)
                return GetDefaultTileData();
            return _methodParams;
        }

        public abstract object[] GetDefaultTileData();


        #endregion Methods
    }
}