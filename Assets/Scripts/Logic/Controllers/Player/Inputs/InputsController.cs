using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class InputsController : MonoBehaviour
    {
        #region Singleton

        [HideInInspector] public static InputsController _instance;

        // Start is called before the first frame update
        private void Awake()
        {
            if (_instance != null)
                DestroyImmediate(this);

            _instance = this;
            DontDestroyOnLoad(this);
        }

        #endregion Singleton

        #region Variables

        //Inputs Config
        [Header("Inputs Config")]
        [SerializeField] [RequireInterfaceAttribute(typeof(InputsListener))] private List<UnityEngine.Object> m_inputsListenerObject;
        private List<InputsListener> m_inputsListener
        {
            get
            {
                List<InputsListener> listeners = new List<InputsListener>();
                m_inputsListenerObject.ForEach((item) => listeners.Add(item as InputsListener));
                return listeners;
            }
            set
            {
                m_inputsListener = value;
            }
        }

        //Timers
        [HideInInspector]
        public float m_initialTimeBetweenInputs = 0.8f;
        [HideInInspector] public float m_currentTimeBetweenInputs = 0.05f;

        //Delegates
        public Action<bool> m_OnMovePiece;
        public Action<bool> m_OnDropPiece;
        public Action<bool> m_OnRotatePiece;
        public Action m_OnStorePiece;

        // KeyPressed Record
        private List<TetrisInputs> m_currentPressedInputs = new List<TetrisInputs>();
        private List<TetrisInputs> m_lastInputPressed = new List<TetrisInputs>();
        private int m_timesPressed = 0;

        //Input timing
        private int m_neededTimePresses = 5;

        #endregion Variables

        #region Methods
        #region Unity Events
        private void Update()
        {
            HandleInputs();
        }

        /// <summary>
        /// Handle the input from the user.
        /// </summary>
        public void HandleInputs()
        {
            m_currentPressedInputs = new List<TetrisInputs>();
            m_inputsListener.ForEach((item) =>
            {
                item.GetCurrentInputsPressed().ForEach((inputsPressed) => m_currentPressedInputs.Add(inputsPressed));
            });

            // Movement of piece
            CheckIfIsPressingKey(TetrisInputs.MOVE_LEFT,
                (inputPressed) => CheckContinuousInput(
                    inputPressed,
                    InputsConsts.INITIAL_NEEDED_TIMES_PRESSED,
                    () => m_OnMovePiece?.Invoke(true)
                ));
            CheckIfIsPressingKey(TetrisInputs.MOVE_RIGHT,
                (inputPressed) => CheckContinuousInput(
                    inputPressed,
                    InputsConsts.INITIAL_NEEDED_TIMES_PRESSED,
                    () => m_OnMovePiece?.Invoke(false)
                ));
            // Dropping piece
            CheckIfIsPressingKey(TetrisInputs.SOFT_DROP,
                (inputPressed) => CheckContinuousInput(
                    inputPressed,
                    InputsConsts.INITIAL_NEEDED_TIMES_PRESSED,
                    () => m_OnDropPiece?.Invoke(true)
                ));
            CheckIfIsPressingKey(TetrisInputs.HARD_DROP,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_OnDropPiece?.Invoke(false)
                ));
            // Rotating Pieces
            CheckIfIsPressingKey(TetrisInputs.ROTATE_CLOCKWISE,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_OnRotatePiece?.Invoke(true)
                ));
            CheckIfIsPressingKey(TetrisInputs.ROTATE_COUNTER_CLOCKWISE,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_OnRotatePiece?.Invoke(false)
                ));
            CheckIfIsPressingKey(TetrisInputs.STORE_PIECE,
                (inputPressed) => CheckSingleInput(
                    inputPressed,
                    () => m_OnStorePiece?.Invoke()
                ));
            //No key Pressed
            if (m_currentPressedInputs.Count == 1 && m_currentPressedInputs[0] == TetrisInputs.NONE)
            {
                m_timesPressed = 0;
                m_lastInputPressed = new List<TetrisInputs>();
            }
        }

        #endregion Unity Events

        #region Helpers

        /// <summary>
        /// Checks if the input can be done and if so then execute the passed by function.
        /// </summary>
        /// <param name="input"> The Key Pressed</param>
        /// <param name="initWaitPressedTimesFactor"> The init wait time to start continuous movement.</param>
        /// <param name="normalPressedTimes">The normal pressed times once it has started continous movement</param>
        /// <param name="inputAction">The action to execute.</param>
        private void CheckContinuousInput(TetrisInputs input, int initWaitPressedTimesFactor, Action inputAction)
        {
            if (m_lastInputPressed.Contains(input))
            {
                m_timesPressed++;
                if (m_timesPressed > m_neededTimePresses)
                {
                    inputAction?.Invoke();
                    m_timesPressed = 0;
                    if (m_neededTimePresses != InputsConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT)
                        m_neededTimePresses = InputsConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT;
                }
            }
            else
            {
                m_neededTimePresses = InputsConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT * initWaitPressedTimesFactor;
                inputAction?.Invoke();
                m_timesPressed = 0;
            }

            if (!m_lastInputPressed.Contains(input))
                m_lastInputPressed.Add(input);
        }

        /// <summary>
        /// Checks if the input can be executed.
        /// </summary>
        /// <param name="keyCode"> The Key Pressed</param>
        /// <param name="inputAction">The action to execute.</param>
        private void CheckSingleInput(TetrisInputs keyCode, Action inputAction)
        {
            if (m_lastInputPressed.Contains(keyCode))
                return;

            inputAction?.Invoke();

            if (m_timesPressed != 0)
                m_timesPressed = 0;

            if (!m_lastInputPressed.Contains(keyCode))
                m_lastInputPressed.Add(keyCode);
        }

        private void CheckIfIsPressingKey(TetrisInputs inputToCheck, Action<TetrisInputs> inputPressedAction)
        {
            if (m_currentPressedInputs.Contains(inputToCheck))
                inputPressedAction?.Invoke(inputToCheck);
            else if (m_lastInputPressed.Contains(inputToCheck))
                m_lastInputPressed.Remove(inputToCheck);
        }

        #endregion Helpers

        #endregion Methods
    }
}