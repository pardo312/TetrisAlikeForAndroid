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
        private int _currentScore = 0;
        #endregion Fields

        #region Methods
        public void Init()
        {
            _currentScore = 0;
        }

        public void CleanLineAddScore(int count)
        {
            ChangeScore((int)(count * ((ScoreConsts.SCORE_BY_LINE * (ScoreConsts.SCORE_MULTIPLIER_BY_LINE * count)))));
        }

        public void ChangeScore(int valueToChange)
        {


        }
        #endregion Methods
    }
    public class ScoreConsts
    {
        public const int SCORE_BY_LINE = 100;
        public const float SCORE_MULTIPLIER_BY_LINE = 1.2f;
        public const int SCORE_BY_TETRIS = 1000;
        public const int SCORE_BY_TSPIN = 500;
    }
}
