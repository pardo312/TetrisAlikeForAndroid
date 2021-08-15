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
        #endregion

        #region Init
        void Start()
        {
            if (_boardController == null)
                _boardController = GetComponent<BoardController>();

            InputsController._instance.timeBetweenInputs = _boardController._timeBetweenFalls * 0.5f;
            InputsController._instance._OnMovePiece += MovePiece;
            InputsController._instance._OnDropPiece += DropPiece;
            InputsController._instance._OnRotatePiece += RotatePiece;
            InputsController._instance._OnStorePiece += StorePiece;

        }
        #endregion

        private bool MovePiece(bool toLeft)
        {
            _boardController.userExecutingAction = true;

            int direction = 1;
            if (toLeft)
                direction = -1;

            bool pieceMoved = _boardController.MovePiecesInSomeDirection(0, direction);
            
            _boardController.userExecutingAction = false;
            return pieceMoved;
        }

        private bool DropPiece(bool softDrop)
        {
            return false;
        }

        private bool RotatePiece(bool clockwise)
        {
            return false;
        }

        private bool StorePiece()
        {
            return false;
        }

    }
}