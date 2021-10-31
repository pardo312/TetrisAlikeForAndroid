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
        [SerializeField] private ScoreView _scoreView;
        #endregion Fields

        #region Methods
        public void Init()
        {
            _scoreView.Init();
            _currentScore = 0;
        }

        public void CleanLineAddScore(int numberOfLines)
        {
            int scoreMultiplier = (int)ScoreConsts.SCORE_MULTIPLIER_BY_LINE * numberOfLines;
            int finalAddedScore = (int)(numberOfLines * ((ScoreConsts.SCORE_BY_LINE * scoreMultiplier)));

            //TETRIS
            if (numberOfLines >= 4)
                finalAddedScore += ScoreConsts.SCORE_BY_TETRIS;

            AddScore(finalAddedScore);
        }

        public void AddScore(int extraValue)
        {
            _currentScore += extraValue;
            _scoreView.AddScore(extraValue, _currentScore);
        }

        #endregion Methods
    }
}
