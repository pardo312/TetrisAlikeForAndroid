using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class InputsController : InputsControllerBase<TetrisInputs>
    {
        #region Methods

        #region Unity Events
        public override void Awake()
        {
            base.Awake();

            //Init Actions
            m_actionsDictionary.Add(InputsTetrisActionsConsts.ON_MOVE_PIECE, (_actionParams) => { });
            m_actionsDictionary.Add(InputsTetrisActionsConsts.ON_DROP_PIECE, (_actionParams) => { });
            m_actionsDictionary.Add(InputsTetrisActionsConsts.ON_ROTATE_PIECE, (_actionParams) => { });
            m_actionsDictionary.Add(InputsTetrisActionsConsts.ON_STORE_PIECE, (_actionParams) => { });

            m_neededTimePresses = InputsTetrisConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT;
        }

        private void Update()
        {
            HandleInputs();
        }
        #endregion Unity Events

        #region HandleInput
        /// <summary>
        /// Handle the input from the user.
        /// </summary>
        public override void HandleInputs()
        {
            base.HandleInputs();
            //No key Pressed
            if (m_currentPressedInputs.Count == 1 && m_currentPressedInputs.Contains(TetrisInputs.NONE))
            {
                m_timesPressed = 0;
                m_lastInputPressed = new List<TetrisInputs>();
                return;
            }

            // Movement of piece
            CheckIfIsPressingKey(TetrisInputs.MOVE_LEFT,
                (inputPressed) => CheckContinuousInput(
                    inputPressed,
                    InputsTetrisConsts.INITIAL_NEEDED_TIMES_PRESSED,
                    InputsTetrisConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT,
                    //() => m_OnMovePiece?.Invoke(true)
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_MOVE_PIECE].Invoke(new object[1] { true })
                ));
            CheckIfIsPressingKey(TetrisInputs.MOVE_RIGHT,
                (inputPressed) => CheckContinuousInput(
                    inputPressed,
                    InputsTetrisConsts.INITIAL_NEEDED_TIMES_PRESSED,
                    InputsTetrisConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT,
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_MOVE_PIECE].Invoke(new object[1] { false })
                ));
            // Dropping piece
            CheckIfIsPressingKey(TetrisInputs.SOFT_DROP,
                (inputPressed) => CheckContinuousInput(
                    inputPressed,
                    InputsTetrisConsts.INITIAL_NEEDED_TIMES_PRESSED,
                    InputsTetrisConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT,
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_DROP_PIECE].Invoke(new object[1] { true })
                ));
            CheckIfIsPressingKey(TetrisInputs.HARD_DROP,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_DROP_PIECE].Invoke(new object[1] { false })
                ));
            // Rotating Pieces
            CheckIfIsPressingKey(TetrisInputs.ROTATE_CLOCKWISE,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_ROTATE_PIECE].Invoke(new object[1] { true })
                ));
            CheckIfIsPressingKey(TetrisInputs.ROTATE_COUNTER_CLOCKWISE,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_ROTATE_PIECE].Invoke(new object[1] { false })
                ));
            CheckIfIsPressingKey(TetrisInputs.STORE_PIECE,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_actionsDictionary[InputsTetrisActionsConsts.ON_STORE_PIECE].Invoke(null)
                ));
        }
        #endregion HandleInput
        #endregion Methods
    }
}