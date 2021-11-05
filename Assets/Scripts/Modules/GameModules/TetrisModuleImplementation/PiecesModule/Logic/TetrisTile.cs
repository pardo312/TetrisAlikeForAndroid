using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class TetrisTile : HideableTileBase
    {
        #region Class Fields
        private SpriteRenderer m_spriteRenderer;
        #endregion Class Fields

        public override void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_tileData = new TetrisTileData() { Color = PieceConsts.DEFAULT_COLOR, IsFilled = false } as object;
        }
        public override object[] GetDefaultTileData()
        {
            return new object[2] {
                 Color.clear,
                 false
            };
        }

        public override void ResetTile()
        {
            base.ResetTile();
            if (m_isPartOfFirstRowAfterRealBoard)
            {
                ChangeTileData();
            }
            else if (!m_isPartOfHiddenBoard)
            {
                ChangeTileData(new object[2] { PieceConsts.DEFAULT_COLOR, false });
            }
        }

        /// <summary>
        /// Change the tile data 
        /// </summary>
        /// <param name="_methodParams">  
        /// 2 Params: Color(Color) and isFilled(bool)
        /// If leaved empty it will fill with default parameters
        /// </param>
        public override object[] ChangeTileData(object[] _methodParams = null)
        {
            _methodParams = base.ChangeTileData(_methodParams);
            TetrisTileData newTileData = m_tileData as TetrisTileData;

            if (_methodParams.Length >= 1 && _methodParams[0] != null)
            {
                if (((m_isPartOfFirstRowAfterRealBoard && newTileData.Color.Equals(PieceConsts.DEFAULT_COLOR)) || m_isPartOfHiddenBoard))
                    newTileData.Color = Color.clear;
                else
                    newTileData.Color = (Color)_methodParams[0];

                m_spriteRenderer.color = newTileData.Color;
            }
            if (_methodParams.Length >= 2 && _methodParams[1] != null)
            {
                newTileData.IsFilled = (bool)_methodParams[1];
            }

            m_tileData = newTileData;
            return _methodParams;
        }
    }
}