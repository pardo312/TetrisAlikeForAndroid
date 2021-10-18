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

            //Implement Adding animation with extra value
            LeanTween.move(_scoreAddedText.gameObject, _scoreValueText.transform,ScoreConsts.ANIMATION_TIME_SCORE_ADDED);

            ChangeScore(finalScore);
        }

        public void ChangeScore(int currentScore)
        {
            _scoreValueText.text = currentScore.ToString();
        }

    }
}
