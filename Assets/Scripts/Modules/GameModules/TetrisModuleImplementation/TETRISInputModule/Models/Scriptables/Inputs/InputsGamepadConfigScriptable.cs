using UnityEngine;

[CreateAssetMenu(fileName = "InputsGamepadConfigScriptable.asset", menuName = "TetrisAlike/Inputs/InputGamepadConfigScriptable")]
public class InputsGamepadConfigScriptable : ScriptableObject
{
    public KeyCode m_none = KeyCode.None;

    public string m_XAxis = "";
    public string m_YAxis = "";

    public string m_altXAxis = "";
    public string m_altYAxis = "";

    public KeyCode m_rotateClockwise = KeyCode.Joystick1Button0;
    public KeyCode m_rotateCounterClockwise = KeyCode.Joystick1Button1;

    public KeyCode m_storePiece = KeyCode.Joystick1Button4;
    public KeyCode m_storePieceAlt = KeyCode.Joystick1Button6;
}