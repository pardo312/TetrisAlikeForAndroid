using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public static class ScoreConsts
    {
        public const int SCORE_BY_LINE = 100;
        public const float SCORE_MULTIPLIER_BY_LINE = 1.2f;
        public const int SCORE_BY_TETRIS = 1000;
        public const int SCORE_BY_TSPIN = 500;

        //Score View
        public const float ANIMATION_TIME_SCORE_ADDED_POSITION = 2f;
        public const float ANIMATION_TIME_SCORE_ADDED_SCALE = 2f;

        public const float ANIMATION_VALUE_SCORE_ADDED_SCALE = 2f;
    }
}
