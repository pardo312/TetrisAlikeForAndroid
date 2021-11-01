using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{

    public abstract class InputsControllerBase<T> : MonoBehaviour, IInputsController<T>
    {

#if !NET_STANDARD_2_0
        #region Singleton

                private static IInputsController<T> m_instanceField;
                public static IInputsController<T> m_instance => m_instaceField;
                private protected virtual void Awake()
                {
                    //Change This
                    if (m_instanceField == null)
                    {
                        m_instanceField = this;
                    }
                    else
                    {
                        DestroyImmediate(this.gameObject);
                    }
                }
        #endregion Singleton
#else
        #region Singleton

        public static IInputsController<T> m_instance;
        public virtual void Awake()
        {
            //Change This
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }
        #endregion Singleton
#endif

        #region Fields
        #region Backing Fields
        //Inputs Config
        [Header("Inputs Config")] [SerializeField] private List<UnityEngine.Object> m_inputsListenerField;
        private float m_initialTimeBetweenInputsField;
        private float m_currentTimeBetweenInputsField;
        private List<T> m_currentPressedInputsField;
        private List<T> m_lastInputPressedField;
        private int m_neededTimePressesField;
        private float m_timesPressedField;
        private Dictionary<string, Action<object[]>> m_actionsDictionaryField = new Dictionary<string, Action<object[]>>();
        #endregion Backing Fields

        #region Properties
        public List<InputsListener<T>> m_inputsListener
        {
            get
            {
                List<InputsListener<T>> listeners = new List<InputsListener<T>>();
                int i = 0;
                m_inputsListenerField.ForEach(
                    (item) =>
                    {
                        Type itemType = item.GetType();
                        if (item is InputsListener<T>)
                        {
                            listeners.Add(item as InputsListener<T>);
                        }
                        else
                        {
                            Debug.LogError($"<color=red>JiufenInputsController:</color>  The item: [{item.name}], in position [{i}], is not an inputListener");
                            Debug.Break();
                        }
                        i++;
                    });
                return listeners;
            }
        }
        public float m_initialTimeBetweenInputs { get => m_initialTimeBetweenInputsField; set => m_initialTimeBetweenInputsField = value; }
        public float m_currentTimeBetweenInputs { get => m_currentTimeBetweenInputsField; set => m_currentTimeBetweenInputsField = value; }
        public List<T> m_currentPressedInputs { get => m_currentPressedInputsField; set => m_currentPressedInputsField = value; }
        public List<T> m_lastInputPressed { get => m_lastInputPressedField; set => m_lastInputPressedField = value; }
        public int m_neededTimePresses { get => m_neededTimePressesField; set => m_neededTimePressesField = value; }
        public float m_timesPressed { get => m_timesPressedField; set => m_timesPressedField = value; }
        public Dictionary<string, Action<object[]>> m_actionsDictionary { get => m_actionsDictionaryField; set => m_actionsDictionaryField = value; }
        #endregion Properties
        #endregion Fields

        #region Methods
        #region OnUpdate
        public virtual void HandleInputs()
        {
            m_currentPressedInputs = new List<T>();
            m_inputsListener.ForEach((item) =>
            {
                item.GetCurrentInputsPressed().ForEach((inputsPressed) =>
                {
                    if (!m_currentPressedInputs.Contains(inputsPressed))
                        m_currentPressedInputs.Add(inputsPressed);
                });
            });

        }
        #endregion OnUpdate

        #region Helpers
        /// <summary>
        /// Checks if the input can be executed.
        /// </summary>
        /// <param name="keyCode"> The Key Pressed</param>
        /// <param name="inputAction">The action to execute.</param>
        public virtual void CheckSingleInput(T keyCode, Action inputAction)
        {
            if (m_lastInputPressed.Contains(keyCode))
                return;

            inputAction?.Invoke();

            if (m_timesPressed != 0)
                m_timesPressed = 0;

            if (!m_lastInputPressed.Contains(keyCode))
                m_lastInputPressed.Add(keyCode);
        }

        /// <summary>
        /// Checks if the input can be done and if so then execute the passed by function.
        /// </summary>
        /// <param name="input"> The Key Pressed</param>
        /// <param name="initWaitPressedTimesFactor"> The init wait time to start continuous movement.</param>
        /// <param name="onContinousMovementNeededPressesForNextMovement">The normal pressed times once it has started continous movement</param>
        /// <param name="inputAction">The action to execute.</param>
        public virtual void CheckContinuousInput(T input, int initWaitPressedTimesFactor, int onContinousMovementNeededPressesForNextMovement, Action inputAction)
        {
            if (m_lastInputPressed.Contains(input))
            {
                m_timesPressed += Time.deltaTime * 100;
                if (m_timesPressed > m_neededTimePresses)
                {
                    inputAction?.Invoke();
                    m_timesPressed = 0;
                    if (m_neededTimePresses != onContinousMovementNeededPressesForNextMovement)
                        m_neededTimePresses = onContinousMovementNeededPressesForNextMovement;
                }
            }
            else
            {
                m_neededTimePresses = onContinousMovementNeededPressesForNextMovement * initWaitPressedTimesFactor;
                inputAction?.Invoke();
                m_timesPressed = 0;
            }

            if (!m_lastInputPressed.Contains(input))
                m_lastInputPressed.Add(input);
        }

        public virtual void CheckIfIsPressingKey(T inputToCheck, Action<T> inputPressedAction)
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