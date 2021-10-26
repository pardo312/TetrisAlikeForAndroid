using UnityEngine;

[CreateAssetMenu(fileName = "InputsConfigScriptable.asset", menuName = "TetrisAlike/Inputs/InputConfigScriptable")]
public class InputsKeyboardConfigScriptable : ScriptableObject
{
    public KeyCode m_none = KeyCode.None;

    public KeyCode m_moveLeft = KeyCode.A;
    public KeyCode m_moveRight = KeyCode.D;

    public KeyCode m_softDrop = KeyCode.S;
    public KeyCode m_hardDrop = KeyCode.W;

    public KeyCode m_rotateClockwise = KeyCode.K;
    public KeyCode m_rotateCounterClockwise = KeyCode.J;

    public KeyCode m_storePiece = KeyCode.E;
}
