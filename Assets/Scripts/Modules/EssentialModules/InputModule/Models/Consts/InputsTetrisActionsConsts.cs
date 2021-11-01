using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public static class InputsTetrisActionsConsts
    {
        #region Actions
        public const string ON_MOVE_PIECE = "OnMovePiece(bool)";
        public const string ON_DROP_PIECE = "OnDropPiece(bool)";
        public const string ON_ROTATE_PIECE = "OnRotatePiece(bool)";
        public const string ON_STORE_PIECE = "OnStorePiece()";
        #endregion Actions
    }
}