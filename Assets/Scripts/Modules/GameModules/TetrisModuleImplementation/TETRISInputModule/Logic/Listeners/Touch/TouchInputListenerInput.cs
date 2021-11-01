using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class TouchInputListenerInput : MonoBehaviour, InputsListener<TetrisInputs>
    {
        [SerializeField] private InputsKeyboardConfigScriptable keysScriptable;
        private List<TetrisInputs> currentlyPressedKeys = new List<TetrisInputs>();

        public List<TetrisInputs> GetCurrentInputsPressed()
        {
            if (currentlyPressedKeys.Count == 0)
                currentlyPressedKeys.Add(TetrisInputs.NONE);

            return currentlyPressedKeys;
        }

        public void PressedButton(string keyCode)
        {
            if (keyCode.CompareTo("MOVE_LEFT") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.MOVE_LEFT))
                currentlyPressedKeys.Add(TetrisInputs.MOVE_LEFT);

            if (keyCode.CompareTo("MOVE_RIGHT") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.MOVE_RIGHT))
                currentlyPressedKeys.Add(TetrisInputs.MOVE_RIGHT);

            if (keyCode.CompareTo("ROTATE_CLOCKWISE") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.ROTATE_CLOCKWISE))
                currentlyPressedKeys.Add(TetrisInputs.ROTATE_CLOCKWISE);

            if (keyCode.CompareTo("ROTATE_COUNTER_CLOCKWISE") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.ROTATE_COUNTER_CLOCKWISE))
                currentlyPressedKeys.Add(TetrisInputs.ROTATE_COUNTER_CLOCKWISE);

            if (keyCode.CompareTo("SOFT_DROP") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.SOFT_DROP))
                currentlyPressedKeys.Add(TetrisInputs.SOFT_DROP);

            if (keyCode.CompareTo("HARD_DROP") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.HARD_DROP))
                currentlyPressedKeys.Add(TetrisInputs.HARD_DROP);

            if (keyCode.CompareTo("STORE_PIECE") == 0 && !currentlyPressedKeys.Contains(TetrisInputs.STORE_PIECE))
                currentlyPressedKeys.Add(TetrisInputs.STORE_PIECE);
        }

        public void UnPressedButton(string keyCode)
        {
            if (keyCode.CompareTo("MOVE_LEFT") == 0 && currentlyPressedKeys.Contains(TetrisInputs.MOVE_LEFT))
                currentlyPressedKeys.Remove(TetrisInputs.MOVE_LEFT);

            if (keyCode.CompareTo("MOVE_RIGHT") == 0 && currentlyPressedKeys.Contains(TetrisInputs.MOVE_RIGHT))
                currentlyPressedKeys.Remove(TetrisInputs.MOVE_RIGHT);

            if (keyCode.CompareTo("ROTATE_CLOCKWISE") == 0 && currentlyPressedKeys.Contains(TetrisInputs.ROTATE_CLOCKWISE))
                currentlyPressedKeys.Remove(TetrisInputs.ROTATE_CLOCKWISE);

            if (keyCode.CompareTo("ROTATE_COUNTER_CLOCKWISE") == 0 && currentlyPressedKeys.Contains(TetrisInputs.ROTATE_COUNTER_CLOCKWISE))
                currentlyPressedKeys.Remove(TetrisInputs.ROTATE_COUNTER_CLOCKWISE);

            if (keyCode.CompareTo("SOFT_DROP") == 0 && currentlyPressedKeys.Contains(TetrisInputs.SOFT_DROP))
                currentlyPressedKeys.Remove(TetrisInputs.SOFT_DROP);

            if (keyCode.CompareTo("HARD_DROP") == 0 && currentlyPressedKeys.Contains(TetrisInputs.HARD_DROP))
                currentlyPressedKeys.Remove(TetrisInputs.HARD_DROP);

            if (keyCode.CompareTo("STORE_PIECE") == 0 && currentlyPressedKeys.Contains(TetrisInputs.STORE_PIECE))
                currentlyPressedKeys.Remove(TetrisInputs.STORE_PIECE);
        }
    }
}