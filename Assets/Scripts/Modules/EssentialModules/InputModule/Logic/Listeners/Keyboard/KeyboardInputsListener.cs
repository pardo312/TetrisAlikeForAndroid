using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class KeyboardInputsListener : MonoBehaviour, InputsListener<TetrisInputs>
    {
        [SerializeField] private InputsKeyboardConfigScriptable keysScriptable;

        public List<TetrisInputs> GetCurrentInputsPressed()
        {
            List<TetrisInputs> currentlyPressedKeys = new List<TetrisInputs>();

            if (Input.GetKey(keysScriptable.m_none))
                currentlyPressedKeys.Add(TetrisInputs.NONE);
            else
            {
                if (Input.GetKey(keysScriptable.m_moveLeft))
                    currentlyPressedKeys.Add(TetrisInputs.MOVE_LEFT);

                if (Input.GetKey(keysScriptable.m_moveRight))
                    currentlyPressedKeys.Add(TetrisInputs.MOVE_RIGHT);

                if (Input.GetKey(keysScriptable.m_rotateClockwise))
                    currentlyPressedKeys.Add(TetrisInputs.ROTATE_CLOCKWISE);

                if (Input.GetKey(keysScriptable.m_rotateCounterClockwise))
                    currentlyPressedKeys.Add(TetrisInputs.ROTATE_COUNTER_CLOCKWISE);

                if (Input.GetKey(keysScriptable.m_softDrop))
                    currentlyPressedKeys.Add(TetrisInputs.SOFT_DROP);

                if (Input.GetKey(keysScriptable.m_hardDrop))
                    currentlyPressedKeys.Add(TetrisInputs.HARD_DROP);

                if (Input.GetKey(keysScriptable.m_storePiece))
                    currentlyPressedKeys.Add(TetrisInputs.STORE_PIECE);
            }

            return currentlyPressedKeys;
        }
    }
}