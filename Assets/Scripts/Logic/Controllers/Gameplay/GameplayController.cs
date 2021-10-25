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
        #region Fields
        [Header("Controllers")]
        //Board
        [SerializeField] BoardController m_boardController;
        //Piece
        [SerializeField] NextPieceController m_nextPieceController;
        [SerializeField] StoredPieceController m_storePieceController;
        CurrentPieceController m_currentPieceController = new CurrentPieceController();
        //Other
        PlayerBehaviour m_playerBehaviour = new PlayerBehaviour();
        [SerializeField] ScoreController m_scoreController;

        [Header("PieceSpawn")]
        [HideInInspector] public bool m_shouldSpawnNewPiece = true;
        public PieceSpawner m_pieceSpawner = new PieceSpawner();
        private Piece pieceToSpawn = null;

        [Header("Gameplay")]
        [HideInInspector] public bool m_userExecutingAction = false;
        [SerializeField, Range(0, 20)] public float m_timeBetweenFalls = 0.01f;
        private float m_timer = 20;
        private bool canStorePiece = true;
        #endregion Fields

        #region Methods
        #region Init
        public void Start()
        {
            m_boardController.Init();
            m_pieceSpawner.Init();
            m_currentPieceController.Init(m_boardController);
            m_playerBehaviour.Init(this);
            m_scoreController.Init();
            m_nextPieceController.Init();
            m_storePieceController.Init();

            AudioManager.PlayAudio("OST_MAIN_THEME", new AudioJobOptions(new AudioFadeInfo(true, 1f), null, true));
        }
        #endregion Init

        #region Flow
        void Update()
        {
            //Check if current piece is in final Position
            bool IsPieceInFinalPosition = false;
            if (m_currentPieceController.m_currentPieceTiles != null)
                IsPieceInFinalPosition = m_currentPieceController.CheckIfPieceIsInFinalPosition();

            //Piece Projection
            if (!m_shouldSpawnNewPiece)
            {
                if (!IsPieceInFinalPosition)
                    m_currentPieceController.SeeWhereCurrentPieceIsDropping();
            }

            m_timer += Time.deltaTime;
            if (m_timer < m_timeBetweenFalls)
                return;
            if (m_userExecutingAction)
                return;

            m_timer = 0;

            //Piece droped and finished
            if (IsPieceInFinalPosition)
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
                canStorePiece = true;
            }
            //Piece Spawn
            else if (m_shouldSpawnNewPiece)
            {
                if (pieceToSpawn == null)
                    pieceToSpawn = m_nextPieceController.GetNextPiece();
                SpawnPiece();
            }
            //Drop piece
            else
            {
                //If it isn't spawing or in final position drop the piece.
                m_currentPieceController.DropPieceTile();
            }
        }
        #endregion Flow

        #region Player Behaviours
        private void SpawnPiece()
        {
            if (m_currentPieceController.m_currentPieceTiles != null)
            {
                if (m_currentPieceController.m_currentPieceTiles.Count > 0)
                {
                    m_currentPieceController.ClearCurrentPieceTiles(m_currentPieceController.m_currentPieceTiles.ToArray());
                    m_currentPieceController.ClearCurrentPieceTiles(m_currentPieceController.m_currentProjectionPieces);
                    m_currentPieceController.m_currentPieceTiles = new List<Vector2Int>();
                    m_currentPieceController.m_currentProjectionPieces = new Vector2Int[4];
                }
            }

            m_pieceSpawner.SpawnPiece(BoardConsts.REAL_ROWS, m_boardController._board, pieceToSpawn,
               (currentPiece, piece4x4SquareTiles, currentPieceTiles) =>
               {

                   m_currentPieceController.m_currentPiece = currentPiece;
                   m_currentPieceController.m_piece4x4CubeStartTile = piece4x4SquareTiles;
                   m_currentPieceController.m_currentPieceTiles = currentPieceTiles;
                   pieceToSpawn = null;

                   m_nextPieceController.ShowNextPiece();
                   m_currentPieceController.OnSpawn();
                   m_shouldSpawnNewPiece = false;
               });
        }

        public void StorePiece()
        {
            if (canStorePiece)
            {
                canStorePiece = false;
                m_shouldSpawnNewPiece = true;
                m_currentPieceController.ClearCurrentPieceTiles(m_currentPieceController.m_currentPieceTiles.ToArray());
                m_currentPieceController.ClearCurrentPieceTiles(m_currentPieceController.m_currentProjectionPieces);

                pieceToSpawn = m_storePieceController.StorePiece(m_currentPieceController.m_currentPiece);
            }
        }

        public void HardDropPiece()
        {
            m_currentPieceController.HardDropPiece(() =>
            {
                m_shouldSpawnNewPiece = true;
            });
        }

        public void MovePiecesInSomeDirection(int x, int y)
        {
            m_currentPieceController.MovePiecesInSomeDirection(x, y);
        }

        public void RotatePiece(bool clockwise)
        {
            m_currentPieceController.RotatePiece(clockwise);
        }
        #endregion Player Behaviours
        #endregion Methods
    }
}
