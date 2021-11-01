using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class PlayerBehaviour
    {
        #region Variables

        private GameplayController _gameplayController;

        #endregion Variables

        #region Init

        public void Init(GameplayController gameplayController)
        {
            _gameplayController = gameplayController;
            InputsControllerBase<TetrisInputs>.m_instance.m_initialTimeBetweenInputs = gameplayController.m_timeBetweenFalls * 0.5f;

            DesuscribreInputsEvents();
            SuscribeInputEvents();
        }

        private void DesuscribreInputsEvents()
        {
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_MOVE_PIECE] -= MovePiece;
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_DROP_PIECE] -= DropPiece;
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_ROTATE_PIECE] -= RotatePiece;
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_STORE_PIECE] -= StorePiece;
        }

        private void SuscribeInputEvents()
        {
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_MOVE_PIECE] += MovePiece;
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_DROP_PIECE] += DropPiece;
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_ROTATE_PIECE] += RotatePiece;
            InputsController.m_instance.m_actionsDictionary[InputsTetrisActionsConsts.ON_STORE_PIECE] += StorePiece;
        }

        #endregion Init

        private void MovePiece(object[] _methodParams)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            bool toLeft = false;
            if (_methodParams != null && _methodParams.Length == 1)
            {
                toLeft = (bool)_methodParams[0];
            }
            else
            {
                Debug.LogError("_methodParams of movepiece not sended correctly");
                return;
            }

            _gameplayController.m_userExecutingAction = true;

            int direction = 1;
                if (toLeft)
                    direction = -1;

            _gameplayController.MovePiecesInSomeDirection(0, direction);

            _gameplayController.m_userExecutingAction = false;
        }

        private void DropPiece(object[] _methodParams)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            bool softDrop = false;
            if (_methodParams != null && _methodParams.Length == 1)
            {
                softDrop = (bool)_methodParams[0];
            }
            else
            {
                Debug.LogError("_methodParams of movepiece not sended correctly");
                return;
            }

            _gameplayController.m_userExecutingAction = true;
            if (softDrop)
                _gameplayController.MovePiecesInSomeDirection(-1, 0);
            else
                _gameplayController.HardDropPiece();

            _gameplayController.m_userExecutingAction = false;
        }

        private void RotatePiece(object[] _methodParams)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            bool clockwise = false;
            if (_methodParams != null && _methodParams.Length == 1)
            {
                clockwise = (bool)_methodParams[0];
            }
            else
            {
                Debug.LogError("_methodParams of movepiece not sended correctly");
                return;
            }
            _gameplayController.m_userExecutingAction = true;
            _gameplayController.RotatePiece(clockwise);
            _gameplayController.m_userExecutingAction = false;
        }

        private void StorePiece(object[] _methodParams)
        {
            if (_gameplayController.m_shouldSpawnNewPiece)
                return;

            _gameplayController.m_userExecutingAction = true;
            _gameplayController.StorePiece();
            _gameplayController.m_userExecutingAction = false;
        }
    }
}