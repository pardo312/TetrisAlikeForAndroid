using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreController : ScoreControllerBase
    {
        #region Methods
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
        #endregion Methods
    }
}