using JiufenGames.TetrisAlike.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class PlayerBehaviour
    {
        #region Variables
        private GameplayController _gameplayController;
        #endregion

        #region Init
        public void Init(GameplayController gameplayController)
        {
            _gameplayController = gameplayController;
            InputsController._instance._initialTimeBetweenInputs = gameplayController.m_timeBetweenFalls * 0.5f;

            DesuscribreInputsEvents();
            SuscribeInputEvents();

        }
        private void DesuscribreInputsEvents()
        {
            InputsController._instance._OnMovePiece -= MovePiece;
            InputsController._instance._OnDropPiece -= DropPiece;
            InputsController._instance._OnRotatePiece -= RotatePiece;
            InputsController._instance._OnStorePiece -= StorePiece;
        }
        private void SuscribeInputEvents()
        {
            InputsController._instance._OnMovePiece += MovePiece;
            InputsController._instance._OnDropPiece += DropPiece;
            InputsController._instance._OnRotatePiece += RotatePiece;
            InputsController._instance._OnStorePiece += StorePiece;
        }
        #endregion

        private void MovePiece(bool toLeft)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            _gameplayController.m_userExecutingAction = true;

            int direction = 1;
            if (toLeft)
                direction = -1;

            _gameplayController.MovePiecesInSomeDirection(0, direction);

            _gameplayController.m_userExecutingAction = false;
        }

        private void DropPiece(bool softDrop)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            _gameplayController.m_userExecutingAction = true;
            if (softDrop)
                _gameplayController.MovePiecesInSomeDirection(-1, 0);
            else
                _gameplayController.HardDropPiece();

            _gameplayController.m_userExecutingAction = false;
        }

        private void RotatePiece(bool clockwise)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            _gameplayController.m_userExecutingAction = true;
            _gameplayController.RotatePiece(clockwise);
            _gameplayController.m_userExecutingAction = false;
        }

        private void StorePiece()
        {

            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            _gameplayController.m_userExecutingAction = true;
            _gameplayController.StorePiece();
            _gameplayController.m_userExecutingAction = false;
        }

    }
}