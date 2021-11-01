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

    }
}
