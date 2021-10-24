using Jiufen.Audio;
using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class GameplayController : MonoBehaviour
    {
        [Header("Controllers")]
        //Board
        [SerializeField] BoardController m_boardController;
        //Piece
        [SerializeField] NextPieceController m_nextPieceController;
        CurrentPieceController m_currentPieceController = new CurrentPieceController();
        //Other
        PlayerBehaviour m_playerBehaviour = new PlayerBehaviour();
        [SerializeField] ScoreController m_scoreController;

        [Header("PieceSpawn")]
        [HideInInspector] public bool m_shouldSpawnNewPiece = true;
        public PieceSpawner m_pieceSpawner = new PieceSpawner();

        [Header("Gameplay")]
        [HideInInspector] public bool m_userExecutingAction = false;
        [SerializeField, Range(0, 20)] public float m_timeBetweenFalls = 0.01f;
        private float m_timer = 20;

        public void Start()
        {
            m_boardController.Init();
            m_pieceSpawner.Init();
            m_currentPieceController.Init(m_boardController);
            m_playerBehaviour.Init(this);
            m_scoreController.Init();
            m_nextPieceController.Init();

            AudioManager.PlayAudio("OST_MAIN_THEME", new AudioJobOptions(null, null, true));
        }

        void Update()
        {
            m_timer += Time.deltaTime;
            if (m_timer < m_timeBetweenFalls)
                return;
            if (m_userExecutingAction)
                return;

            m_timer = 0;

            if (m_shouldSpawnNewPiece)
            {
                SpawnPiece();
                m_shouldSpawnNewPiece = false;
            }
            else if (m_currentPieceController.CheckIfPieceIsInFinalPosition())
            {
                m_userExecutingAction = true;

                List<int> filledRows = m_currentPieceController.CheckTileBelow(ref m_shouldSpawnNewPiece);
                if (filledRows.Count > 0)
                {
                    m_boardController.ClearCompletedLine(filledRows);
                    m_scoreController.CleanLineAddScore(filledRows.Count);
                }

                m_userExecutingAction = false;

                m_timer = m_timeBetweenFalls;
            }
            else
            {
                //If it isn't spawing or in final position drop the piece.
                m_currentPieceController.DropPieceTile();
            }
            m_playerBehaviour.NeedToWaitForNextSpawn();

        }

        private void SpawnPiece()
        {
            m_pieceSpawner.SpawnPiece(BoardConsts.REAL_ROWS, m_boardController._board, m_nextPieceController.GetNextPiece(),
               (currentPiece, piece4x4SquareTiles, currentPieceTiles) =>
               {
                   m_currentPieceController._currentPiece = currentPiece;
                   m_currentPieceController._piece4x4CubeStartTile = piece4x4SquareTiles;
                   m_currentPieceController._currentPieceTiles = currentPieceTiles;
               });

            m_nextPieceController.ShowNextPiece();
            m_currentPieceController.OnSpawn();

        }

        public void HardDropPiece()
        {
            m_currentPieceController.HardDropPiece();
        }

        public void MovePiecesInSomeDirection(int x, int y)
        {
            m_currentPieceController.MovePiecesInSomeDirection(x, y);
        }

        public void RotatePiece(bool clockwise)
        {
            m_currentPieceController.RotatePiece(clockwise);
        }
    }
}
