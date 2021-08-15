using UnityEngine;

[CreateAssetMenu(fileName = "InputsConfigScriptable.asset", menuName = "TetrisAlike/Inputs/InputsConfigScriptable")]
public class InputsConfigScriptable: ScriptableObject
{
    public InputSource _inputSource = InputSource.KEYBOARD;

    public KeyCode _moveLeft = KeyCode.A;
    public KeyCode _moveRight = KeyCode.D;

    public KeyCode _softDrop = KeyCode.S;
    public KeyCode _hardDrop = KeyCode.W;

    public KeyCode _rotateClockwise = KeyCode.K;
    public KeyCode _rotateCounterClockwise = KeyCode.J;

    public KeyCode _storePiece = KeyCode.E;

}
