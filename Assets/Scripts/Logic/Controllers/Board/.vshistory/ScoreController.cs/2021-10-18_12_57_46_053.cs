using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreController
    {
        #region Fields
        private int _scoreBy
        #endregion Fields

        #region Methods
        public void Init()
        {

        }

        public void CleanLineAddScore(int count)
        {
        }
        #endregion Methods
    }
    public class ScoreConsts
    {
        public const int SCORE_BY_LINE = 100;
        public const int SCORE_BY_TETRIS = 1000;
        public const int SCORE_BY_TSPIN = 500;

    }
}
