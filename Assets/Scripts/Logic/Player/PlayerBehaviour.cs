using JiufenGames.TetrisAlike.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class PlayerBehaviour : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameplayController _gameplayController;
        private bool _needToWaitForNextSpawn = false;
        #endregion

        #region Init
        void Start()
        {
            if (_gameplayController == null)
                _gameplayController = GetComponent<GameplayController>();

            InputsController._instance._initialTimeBetweenInputs = _gameplayController._timeBetweenFalls * 0.5f;
            InputsController._instance._OnMovePiece += MovePiece;
            InputsController._instance._OnDropPiece += DropPiece;
            InputsController._instance._OnRotatePiece += RotatePiece;
            InputsController._instance._OnStorePiece += StorePiece;

        }
        #endregion

        private void Update()
        {
            // Check if the nextPiece has spawned
            if (_needToWaitForNextSpawn )
                _needToWaitForNextSpawn  = !_gameplayController._shouldSpawnNewPiece;
        }

        private void MovePiece(bool toLeft)
        {
            if (_needToWaitForNextSpawn)
                return;
            _gameplayController.userExecutingAction = true;

            int direction = 1;
            if (toLeft)
                direction = -1;

            _gameplayController.MovePiecesInSomeDirection(0, direction);
            
            _gameplayController.userExecutingAction = false;
        }

        private void DropPiece(bool softDrop)
        {
            if (_needToWaitForNextSpawn)
                return;

            _gameplayController.userExecutingAction = true;
            if (softDrop)
                _gameplayController.MovePiecesInSomeDirection(-1,0);
            else
            {
                _gameplayController.HardDropPiece();
                if(!_gameplayController._shouldSpawnNewPiece)
                    _needToWaitForNextSpawn = true;
            }
            
            _gameplayController.userExecutingAction = false;
        }

        private void RotatePiece(bool clockwise)
        {
            if (_needToWaitForNextSpawn)
                return;

            _gameplayController.userExecutingAction = true;
            _gameplayController.RotatePiece(clockwise);
            _gameplayController.userExecutingAction = false;
        }

        private void StorePiece()
        {
            if (_needToWaitForNextSpawn)
                return;
            return;
        }

    }
}