using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreController : MonoBehaviour
    {
        #region Fields
        private int _currentScore = 0;
        #endregion Fields

        #region Methods
        public void Init()
        {
            _currentScore = 0;
        }

        public void CleanLineAddScore(int numberOfLines)
        {
            int scoreMultiplier = (int)ScoreConsts.SCORE_MULTIPLIER_BY_LINE * numberOfLines;
            AddScore((int)(numberOfLines * ((ScoreConsts.SCORE_BY_LINE * scoreMultiplier))));

            //TETRIS
            if (numberOfLines >= 4)
                AddScore(ScoreConsts.SCORE_BY_TETRIS);
        }

        public void AddScore(int extraValue)
        {
            _currentScore += extraValue;
        }

        #endregion Methods
    }
}
