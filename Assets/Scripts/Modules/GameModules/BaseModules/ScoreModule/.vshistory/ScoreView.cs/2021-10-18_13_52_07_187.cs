using System;
using TMPro;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreValueText;
        [SerializeField] private Transform _rectParentScore;

        public void Init()
        {
            _rectParentScore.gameObject.SetActive(true);
        }

        public void AddScore(int extraValue, int finalScore)
        {

            //Implement Adding animation with extra value

            ChangeScore(finalScore);
        }

        public void ChangeScore(int currentScore)
        {
            _scoreValueText.text = currentScore.ToString();
        }

    }
}
