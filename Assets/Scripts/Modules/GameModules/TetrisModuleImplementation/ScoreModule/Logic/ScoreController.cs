using JiufenModules.ScoreModule.Logic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreController : ScoreControllerBase
    {
        #region Methods
        private int highScore = 0;
        public override void Init(object _initialData)
        {
            base.Init(_initialData);
            ScoreData scoreData = _initialData as ScoreData;
            highScore = scoreData.highScore;
            ChangeScore(scoreData.initScore);
        }

        public void CleanLineAddScore(int numberOfLines)
        {
            int scoreMultiplier = (int)ScoreConsts.SCORE_MULTIPLIER_BY_LINE * numberOfLines;
            int finalAddedScore = (int)(numberOfLines * ((ScoreConsts.SCORE_BY_LINE * scoreMultiplier)));

            //TETRIS
            if (numberOfLines >= 4)
                finalAddedScore += ScoreConsts.SCORE_BY_TETRIS;

            //Add Score
            AddScore(finalAddedScore);
        }

        public int GetHighscore()
        {
            if(m_currentScore > highScore)
                return (int)m_currentScore;
            else
                return highScore;
        }
        #endregion Methods
    }
}