using JiufenGames.TetrisAlike.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class PlayerBehaviour : MonoBehaviour
    {
        #region Variables
        [SerializeField] private BoardController _boardController;
        private bool _needToWaitForNextSpawn = false;
        #endregion

        #region Init
        void Start()
        {
            if (_boardController == null)
                _boardController = GetComponent<BoardController>();

            InputsController._instance._initialTimeBetweenInputs = _boardController._timeBetweenFalls * 0.5f;
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
                _needToWaitForNextSpawn  = !_boardController._shouldSpawnNewPiece;
        }

        private void MovePiece(bool toLeft)
        {
            if (_needToWaitForNextSpawn)
                return;
            _boardController.userExecutingAction = true;

            int direction = 1;
            if (toLeft)
                direction = -1;

            _boardController.MovePiecesInSomeDirection(0, direction);
            
            _boardController.userExecutingAction = false;
        }

        private void DropPiece(bool softDrop)
        {
            if (_needToWaitForNextSpawn)
                return;

            _boardController.userExecutingAction = true;
            if (softDrop)
                _boardController.MovePiecesInSomeDirection(-1,0);
            else
            {
                _boardController.HardDropPiece();
                _needToWaitForNextSpawn = true;
            }
            
            _boardController.userExecutingAction = false;
        }

        private void RotatePiece(bool clockwise)
        {
            if (_needToWaitForNextSpawn)
                return;

            _boardController.userExecutingAction = true;
            _boardController.RotatePiece(clockwise);
            _boardController.userExecutingAction = false;
        }

        private void StorePiece()
        {
            if (_needToWaitForNextSpawn)
                return;
            return;
        }

    }
}