using Jiufen.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{

    public enum TetrisInputs
    {
        NONE,
        MOVE_LEFT,
        MOVE_RIGHT,
        ROTATE_CLOCKWISE,
        ROTATE_COUNTER_CLOCKWISE,
        SOFT_DROP,
        HARD_DROP,
        STORE_PIECE
    }
}
