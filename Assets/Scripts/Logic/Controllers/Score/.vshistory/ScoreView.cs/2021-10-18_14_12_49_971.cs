using System;
using TMPro;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Transform _rectParentScore;
        [SerializeField] private TMP_Text _scoreValueText;
        [SerializeField] private TMP_Text _scoreAddedText;

        public void Init()
        {
            _rectParentScore.gameObject.SetActive(true);
        }

        public void AddScore(int extraValue, int finalScore)
        {
            ScoreAddedAnimation(extraValue);
            ChangeScore(finalScore);
        }

        private void ScoreAddedAnimation(int extraValue)
        {
            _scoreAddedText.gameObject.SetActive(true);
            _scoreAddedText.text = extraValue.ToString();

            //Implement Adding animation with extra value
            Vector3 previousPosition = _scoreAddedText.transform.position;
            LeanTween.move(_scoreAddedText.gameObject, _scoreValueText.transform, ScoreConsts.ANIMATION_TIME_SCORE_ADDED_POSITION).setOnComplete(() =>
               {
                   Vector3 previoursScale = _scoreAddedText.transform.localScale;

                   LeanTween.scale(_scoreAddedText.gameObject,
                       new Vector3(ScoreConsts.ANIMATION_VALUE_SCORE_ADDED_SCALE, ScoreConsts.ANIMATION_VALUE_SCORE_ADDED_SCALE, 0),
                       ScoreConsts.ANIMATION_TIME_SCORE_ADDED_SCALE / 2
                       ).setOnComplete(() =>
                       {
                           LeanTween.scale(_scoreAddedText.gameObject, previoursScale, ScoreConsts.ANIMATION_TIME_SCORE_ADDED_SCALE / 2);
                       });
               });
            LeanTween.alpha(_scoreAddedText.gameObject, 0, ScoreConsts.ANIMATION_TIME_SCORE_ADDED_POSITION + ScoreConsts.ANIMATION_TIME_SCORE_ADDED_SCALE).setOnComplete(() =>
                {
                    _scoreAddedText.gameObject.SetActive(false);
                    _scoreAddedText.gameObject.LeanAlpha(1, 0);
                    _scoreAddedText.transform.position = previousPosition;
                });

        }

        public void ChangeScore(int currentScore)
        {
            _scoreValueText.text = currentScore.ToString();
        }

    }
}
