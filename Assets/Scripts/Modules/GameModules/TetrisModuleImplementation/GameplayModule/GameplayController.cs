using Jiufen.Audio;
using JiufenGames.TetrisAlike.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenGames.TetrisAlike.Logic
{
    public class GameplayController : MonoBehaviour
    {
        #region Fields

        [Header("Controllers")]
        //Board
        [SerializeField] private BoardController m_boardController;

        //Piece
        [SerializeField] private NextPieceController m_nextPieceController;

        [SerializeField] private StoredPieceController m_storePieceController;
        private CurrentPieceController m_currentPieceController = new CurrentPieceController();


        [SerializeField] private ScoreController m_scoreController;

        [Header("PieceSpawn")]
        [HideInInspector] public bool m_shouldSpawnNewPiece = true;

        private Piece pieceToSpawn = null;

        [Header("Gameplay")]
        [HideInInspector] public bool m_userExecutingAction = false;

        [SerializeField, Range(0, 20)] public float m_timeBetweenFalls = 0.01f;
        private float m_timer = 20;
        private float timesMovementHasBeenMade = 0;
        private bool canStorePiece = true;

        #endregion Fields

        #region Methods

        #region Init

        public void Init(int highscore)
        {
            m_boardController.Init();
            m_currentPieceController.Init(m_boardController);
            m_scoreController.Init(highscore);
            m_nextPieceController.Init();
            m_storePieceController.Init();

            StartCoroutine(PlayInitialMusic());
        }

        IEnumerator PlayInitialMusic()
        {
            yield return new WaitForEndOfFrame();
            AudioManager.PlayAudio("OST_MAIN_THEME", new AudioJobOptions(new AudioFadeInfo(true, 1f), null, true));
        }

        #endregion Init

        #region Flow

        private void Update()
        {
            //Check if current piece is in final Position
            bool IsPieceInFinalPosition = false;
            if (m_currentPieceController.m_currentPieceTiles != null)
                IsPieceInFinalPosition = m_currentPieceController.CheckIfPieceIsInFinalPosition();

            //Piece Projection
            if (!m_shouldSpawnNewPiece)
                m_currentPieceController.SeeWhereCurrentPieceIsDropping();

            m_timer += Time.deltaTime;
            if (m_timer < m_timeBetweenFalls)
                return;
            if (m_userExecutingAction)
                return;
            m_timer = 0;

            //Piece Spawn
            if (m_shouldSpawnNewPiece)
            {
                if (pieceToSpawn == null)
                    pieceToSpawn = m_nextPieceController.GetNextPiece();
                SpawnPiece();
            }
            //Piece droped and finished
            else if (IsPieceInFinalPosition)
            {
                if (!GivePlayerAChance())
                    return;

                FillRow();
            }
            //Drop piece
            else
            {
                //If it isn't spawing or in final position drop the piece.
                m_currentPieceController.DropPieceTile();
            }
        }

        private void FillRow()
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

        private bool GivePlayerAChance()
        {
            if (timesMovementHasBeenMade < 1)
            {
                m_timer = 0f;
                timesMovementHasBeenMade += Time.deltaTime * 100;
                return false;
            }
            timesMovementHasBeenMade = 0;
            return true;
        }

        public void ResetScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        #endregion Flow

        #region Player Behaviours

        private void SpawnPiece()
        {
            if (m_currentPieceController.m_currentPieceTiles != null)
            {
                if (m_currentPieceController.m_currentPieceTiles.Count > 0)
                {
                    m_currentPieceController.m_currentPieceTiles = new List<Vector2Int>();
                    m_currentPieceController.m_currentProjectionPieces = new Vector2Int[4];
                }
            }

            m_boardController.SpawnPiece(BoardConsts.REAL_ROWS, 6, pieceToSpawn,
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
                m_currentPieceController.ClearCurrentPieceTiles(m_currentPieceController.m_currentPieceTiles.ToArray());
                m_currentPieceController.ClearCurrentPieceTiles(m_currentPieceController.m_currentProjectionPieces);
                pieceToSpawn = m_storePieceController.StorePiece(m_currentPieceController.m_currentPiece);
                canStorePiece = false;
                m_shouldSpawnNewPiece = true;
            }
        }

        public void HardDropPiece()
        {
            m_currentPieceController.HardDropPiece(() =>
            {
                FillRow();
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