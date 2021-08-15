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
    [Header("Inputs Config")]
    [SerializeField] private InputsConfigScriptable _inputsConfig;

    //Timers
    [HideInInspector] public float timeBetweenInputs = 0.1f;
    [HideInInspector] public float currentTimeBetweenInputs = 0.1f;
    private float timer = 0;

    //Delegates 
    public Func<bool,bool> _OnMovePiece;
    public Func<bool,bool> _OnDropPiece;
    public Func<bool,bool> _OnRotatePiece;
    public Func<bool> _OnStorePiece;

    #endregion

    #region Update

    private float timesUserHasClicked = InputsConsts.START_TIMES_USER_HAS_CLICKED;
    private KeyCode lastMovementDone = KeyCode.None;

    private void Update()
    {
        if (HandleInputs())
            timesUserHasClicked *= InputsConsts.PERCENT_OF_WAIT_TIME_FOR_INPUT_DECREASE;
        else if(timer == 0)
            timesUserHasClicked = 1;
    }

    public bool HandleInputs()
    {
        timer += Time.deltaTime;

        if (timer < currentTimeBetweenInputs)
            return false;

        timer = 0;
        currentTimeBetweenInputs = timeBetweenInputs * timesUserHasClicked ;

        bool userDidInput = false;

        // Movement of piece
        if (Input.GetKey(_inputsConfig._moveLeft))
        {
            userDidInput = _OnMovePiece?.Invoke(true) == true &&
                           lastMovementDone == _inputsConfig._moveLeft;
            lastMovementDone = _inputsConfig._moveLeft;
        }
        if (Input.GetKey(_inputsConfig._moveRight))
        {
            userDidInput = _OnMovePiece?.Invoke(false) == true &&
                           lastMovementDone == _inputsConfig._moveRight;
            lastMovementDone = _inputsConfig._moveRight;

        }

        //TODO:

        //// Dropping piece
        //if (Input.GetKey(_inputsConfig._softDrop))
        //   userDidInput = _OnDropPiece?.Invoke(true) == true;
        //if (Input.GetKey(_inputsConfig._softDrop))
        //   userDidInput = _OnDropPiece?.Invoke(false) == true;

        //// Rotating Pieces
        //if (Input.GetKey(_inputsConfig._rotateClockwise))
        //   userDidInput = _OnRotatePiece?.Invoke(true) == true;
        //if (Input.GetKey(_inputsConfig._rotateCounterClockwise))
        //   userDidInput = _OnRotatePiece?.Invoke(false) == true;

        ////Storing Pieces
        //if (Input.GetKey(_inputsConfig._storePiece))
        //   userDidInput = _OnStorePiece?.Invoke() == true;

        return userDidInput;

    }
    #endregion
}
