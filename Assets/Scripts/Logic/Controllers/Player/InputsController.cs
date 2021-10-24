using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsController : MonoBehaviour
{

    #region Singleton 

    [HideInInspector] public static InputsController _instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
            DestroyImmediate(this);

        _instance = this;
        DontDestroyOnLoad(this);
    }

    #endregion

    #region Variables
    //Inputs Config
    [Header("Inputs Config")]
    [SerializeField] private InputsConfigScriptable _inputsConfig;

    //Timers
    [HideInInspector] public float _initialTimeBetweenInputs = 0.8f;
    [HideInInspector] public float _currentTimeBetweenInputs = 0.05f;
    private float _timer = 0;

    //Delegates 
    public Action<bool> _OnMovePiece;
    public Action<bool> _OnDropPiece;
    public Action<bool> _OnRotatePiece;
    public Action _OnStorePiece;

    // KeyPressed Record
    private List<KeyCode> _lastKeyPressed = new List<KeyCode>();
    private int timesPressed = 0;

    //Input timing
    private int _neededTimePresses = 5;

    #endregion

    #region Update

    private void Update()
    {
        HandleInputs();
    }

    /// <summary>
    /// Handle the input from the user.
    /// </summary>
    public void HandleInputs()
    {
        // Movement of piece
        CheckIfIsPressingKey(_inputsConfig._moveLeft,
            () => CheckContinuousInput(
                _inputsConfig._moveLeft,
                InputsConsts.INITIAL_NEEDED_TIMES_PRESSED,
                () => _OnMovePiece?.Invoke(true)
            ));
        CheckIfIsPressingKey(_inputsConfig._moveRight,
            () => CheckContinuousInput(
                _inputsConfig._moveRight,
                InputsConsts.INITIAL_NEEDED_TIMES_PRESSED,
                () => _OnMovePiece?.Invoke(false)
            ));
        // Dropping piece
        CheckIfIsPressingKey(_inputsConfig._softDrop,
            () => CheckContinuousInput(
                _inputsConfig._softDrop,
                InputsConsts.INITIAL_NEEDED_TIMES_PRESSED,
                () => _OnDropPiece?.Invoke(true)
            ));
        CheckIfIsPressingKey(_inputsConfig._hardDrop,
            () => CheckSingleInput(
                _inputsConfig._hardDrop,
                () => _OnDropPiece?.Invoke(false)
            ));
        // Rotating Pieces
        CheckIfIsPressingKey(_inputsConfig._rotateClockwise,
            () => CheckSingleInput(
                _inputsConfig._rotateClockwise,
                () => _OnRotatePiece?.Invoke(true)
            ));
        CheckIfIsPressingKey(_inputsConfig._rotateCounterClockwise,
            () => CheckSingleInput(
                _inputsConfig._rotateCounterClockwise,
                () => _OnRotatePiece?.Invoke(false)
            ));
        //No key Pressed
        if (Input.GetKey(KeyCode.None))
        {
            timesPressed = 0;
            _lastKeyPressed = new List<KeyCode>();
        }

        ////Storing Pieces
        //if (Input.GetKey(_inputsConfig._storePiece))
        //   userDidInput = _OnStorePiece?.Invoke() == true;
    }

    /// <summary>
    /// Checks if the input can be done and if so then execute the passed by function.
    /// </summary>
    /// <param name="keyCode"> The Key Pressed</param>
    /// <param name="initWaitPressedTimesFactor"> The init wait time to start continuous movement.</param>
    /// <param name="normalPressedTimes">The normal pressed times once it has started continous movement</param>
    /// <param name="inputAction">The action to execute.</param>
    private void CheckContinuousInput(KeyCode keyCode, int initWaitPressedTimesFactor, Action inputAction)
    {
        if (_lastKeyPressed.Contains(keyCode))
        {
            timesPressed++;
            if (timesPressed > _neededTimePresses)
            {
                inputAction?.Invoke();
                timesPressed = 0;
                if (_neededTimePresses != InputsConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT)
                    _neededTimePresses = InputsConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT;
            }
        }
        else
        {
            _neededTimePresses = InputsConsts.ON_CONTINOUS_MOVEMENT_NEEDED_TIME_PRESSES_FOR_NEXT_MOVEMENT * initWaitPressedTimesFactor;
            inputAction?.Invoke();
            timesPressed = 0;
        }

        if (!_lastKeyPressed.Contains(keyCode))
            _lastKeyPressed.Add(keyCode);
    }

    /// <summary>
    /// Checks if the input can be executed.
    /// </summary>
    /// <param name="keyCode"> The Key Pressed</param>
    /// <param name="inputAction">The action to execute.</param>
    private void CheckSingleInput(KeyCode keyCode, Action inputAction)
    {

        if (_lastKeyPressed.Contains(keyCode))
            return;

        inputAction?.Invoke();

        if (timesPressed != 0)
            timesPressed = 0;

        if (!_lastKeyPressed.Contains(keyCode))
            _lastKeyPressed.Add(keyCode);
    }

    private void CheckIfIsPressingKey(KeyCode keycode, Action inputCheck)
    {
        if (Input.GetKey(keycode))
            inputCheck?.Invoke();
        else if (_lastKeyPressed.Contains(keycode))
            _lastKeyPressed.Remove(keycode);
    }
    #endregion
}