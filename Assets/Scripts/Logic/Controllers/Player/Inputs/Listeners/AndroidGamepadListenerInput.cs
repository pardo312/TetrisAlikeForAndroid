using Jiufen.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class AndroidGamepadListenerInput : MonoBehaviour, InputsListener
    {
        [SerializeField] InputsGamepadConfigScriptable m_gamepadButtonScriptable;
        public List<TetrisInputs> GetCurrentInputsPressed()
        {
            List<TetrisInputs> currentlyPressedKeys = new List<TetrisInputs>();

            //None
            if (Input.GetKey(m_gamepadButtonScriptable.m_none))
                currentlyPressedKeys.Add(TetrisInputs.NONE);
            else
            {

                //X-Axis
                float XAxis = Input.GetAxis(m_gamepadButtonScriptable.m_XAxis);

                if (XAxis < 0)
                    currentlyPressedKeys.Add(TetrisInputs.MOVE_LEFT);
                else if (XAxis > 0)
                    currentlyPressedKeys.Add(TetrisInputs.MOVE_RIGHT);
                else
                    currentlyPressedKeys.Add(TetrisInputs.NONE);

                //X-Axis Alt
                float altXAxis = Input.GetAxis(m_gamepadButtonScriptable.m_altXAxis);

                if (altXAxis < 0)
                    currentlyPressedKeys.Add(TetrisInputs.MOVE_LEFT);
                else if (altXAxis > 0)
                    currentlyPressedKeys.Add(TetrisInputs.MOVE_RIGHT);
                else
                    currentlyPressedKeys.Add(TetrisInputs.NONE);

                //Rotation
                if (Input.GetKey(m_gamepadButtonScriptable.m_rotateClockwise))
                    currentlyPressedKeys.Add(TetrisInputs.ROTATE_CLOCKWISE);

                if (Input.GetKey(m_gamepadButtonScriptable.m_rotateClockwise))
                    currentlyPressedKeys.Add(TetrisInputs.ROTATE_COUNTER_CLOCKWISE);

                //Y-Axis
                float YAxis = Input.GetAxis(m_gamepadButtonScriptable.m_YAxis);

                if (YAxis < 0)
                    currentlyPressedKeys.Add(TetrisInputs.SOFT_DROP);
                else if (YAxis > 0)
                    currentlyPressedKeys.Add(TetrisInputs.HARD_DROP);
                else
                    currentlyPressedKeys.Add(TetrisInputs.NONE);

                //Y-Axis ALT
                float altYAxis = Input.GetAxis(m_gamepadButtonScriptable.m_YAxis);

                if (altYAxis < 0)
                    currentlyPressedKeys.Add(TetrisInputs.SOFT_DROP);
                else if (altYAxis > 0)
                    currentlyPressedKeys.Add(TetrisInputs.HARD_DROP);
                else
                    currentlyPressedKeys.Add(TetrisInputs.NONE);

                //Store Piece
                if (Input.GetKey(m_gamepadButtonScriptable.m_storePiece))
                    currentlyPressedKeys.Add(TetrisInputs.STORE_PIECE);

                if (Input.GetKey(m_gamepadButtonScriptable.m_storePieceAlt))
                    currentlyPressedKeys.Add(TetrisInputs.STORE_PIECE);
            }

            return currentlyPressedKeys;
        }
    }
}
