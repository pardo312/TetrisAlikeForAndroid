using UnityEngine;
using UnityEngine.Scripting;

namespace JiufenGames.TetrisAlike.Logic
{

    public abstract class ScoreControllerBase :MonoBehaviour, IScoreController
    {
        #region Fields
        #region Backing Fields
        [SerializeField][RequireInterface(typeof(IScoreView))]private UnityEngine.Object m_scoreViewField;
        private float m_currentScoreField;
        #endregion Backing Fields

        #region Properties
        public float m_currentScore { get => m_currentScoreField; private set => m_currentScoreField = value; }
        public IScoreView m_scoreView { get => m_scoreViewField as IScoreView; }
        #endregion Properties
        #endregion Fields


        public virtual void Init(object _initialData)
        {
            m_scoreView.Init(_initialData);
            ResetScore();
        }

        public virtual void AddScore(float _extraValue, object[] _extraParams = null)
        {
            m_currentScore += (int)_extraValue;
            m_scoreView.AddScore(m_currentScore, _extraValue, _extraParams);
        }

        public void RemoveScore(float _removeValue, object[] _extraParams = null)
        {
            m_currentScore -= (int)_removeValue;
            m_scoreView.RemoveScore(m_currentScore, _removeValue, _extraParams);
        }

        public void ResetScore()
        {
            m_currentScore = 0;
            m_scoreView.ChangeScore(0);
        }
    }
}