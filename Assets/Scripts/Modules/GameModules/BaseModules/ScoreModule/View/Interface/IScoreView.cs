using System;
using TMPro;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public interface IScoreView
    {
        void Init(object _initialData);
        void AddScore(float _finalScore, float _extraValue, object[] _extraParams = null);
        void RemoveScore(float _finalScore, float _removeValue, object[] _extraParams = null);
        void ChangeScore(float _finalScore);
    }
}