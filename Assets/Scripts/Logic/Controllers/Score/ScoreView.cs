using System;
using TMPro;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreView : MonoBehaviour
    {
        [Header("Current Score")]
        [SerializeField] private Transform m_rectParentScore;

        [SerializeField] private TMP_Text m_scoreValueText;
        [SerializeField] private TMP_Text m_scoreAddedText;

        [Header("High Score")]
        [SerializeField] private Transform m_rectParentHighScore;

        [SerializeField] private TMP_Text m_scoreValueHighText;

        public void Init(int _highScore)
        {
            //Highscore
            m_rectParentHighScore.gameObject.SetActive(true);
            m_scoreValueHighText.text = _highScore.ToString();

            //Currentscore
            m_rectParentScore.gameObject.SetActive(true);
            ChangeScore(0);
        }

        public void AddScore(int _extraValue, int _finalScore)
        {
            ScoreAddedAnimation(_extraValue, () => { ChangeScore(_finalScore); });
        }

        private void ScoreAddedAnimation(int extraValue, Action onEndAnimation)
        {
            m_scoreAddedText.gameObject.SetActive(true);
            m_scoreAddedText.text = $"+{extraValue.ToString()}";

            //Implement Adding animation with extra value
            Vector3 previousPosition = m_scoreAddedText.transform.position;
            LeanTween.move(m_scoreAddedText.gameObject, m_scoreValueText.transform, ScoreConsts.ANIMATION_TIME_SCORE_ADDED_POSITION).setOnComplete(() =>
               {
                   Vector3 previoursScale = m_scoreAddedText.transform.localScale;

                   LeanTween.scale(m_scoreAddedText.gameObject,
                       new Vector3(ScoreConsts.ANIMATION_VALUE_SCORE_ADDED_SCALE, ScoreConsts.ANIMATION_VALUE_SCORE_ADDED_SCALE, 0),
                       ScoreConsts.ANIMATION_TIME_SCORE_ADDED_SCALE / 2
                       ).setOnComplete(() =>
                       {
                           LeanTween.scale(m_scoreAddedText.gameObject, previoursScale, ScoreConsts.ANIMATION_TIME_SCORE_ADDED_SCALE / 2);
                       });
               });
            LeanTween.alpha(m_scoreAddedText.gameObject, 0, ScoreConsts.ANIMATION_TIME_SCORE_ADDED_POSITION + ScoreConsts.ANIMATION_TIME_SCORE_ADDED_SCALE).setOnComplete(() =>
                {
                    m_scoreAddedText.gameObject.SetActive(false);
                    m_scoreAddedText.gameObject.LeanAlpha(1, 0);
                    m_scoreAddedText.transform.position = previousPosition;
                    onEndAnimation?.Invoke();
                });
        }

        public void ChangeScore(int _currentScore)
        {
            m_scoreValueText.text = _currentScore.ToString();
        }
    }
}