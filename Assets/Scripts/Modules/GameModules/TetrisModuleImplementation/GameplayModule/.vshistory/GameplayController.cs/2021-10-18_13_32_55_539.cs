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
        [SerializeField] BoardController _boardController;
        CurrentPieceController _currentPieceController = new CurrentPieceController();
        ScoreController _scoreController = new ScoreController();
        PlayerBehaviour _playerBehaviour = new PlayerBehaviour();

        [Header("PieceSpawn")]
        [HideInInspector] public bool _shouldSpawnNewPiece = true;
        public PieceSpawner _pieceSpawner = new PieceSpawner();

        [Header("Gameplay")]
        [HideInInspector] public bool userExecutingAction = false;
        [SerializeField, Range(0, 20)] public float _timeBetweenFalls = 0.01f;
        private float _timer = 20;

        public void Start()
        {
            _boardController.Init();
            _pieceSpawner.Init();
            _currentPieceController.Init(_boardController);
            _playerBehaviour.Init(this);
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _timeBetweenFalls)
                return;
            if (userExecutingAction)
                return;

            _timer = 0;


            bool isPieceInFinalPosition = _currentPieceController.CheckIfPieceIsInFinalPosition();

            if (_shouldSpawnNewPiece)
            {
                SpawnPiece();
                _shouldSpawnNewPiece = false;
            }
            else if (!isPieceInFinalPosition)
            {
                //If it isn't spawing or in final position drop the piece.
                _currentPieceController.DropPieceTile();
            }

            if (isPieceInFinalPosition)
            {
                userExecutingAction = true;

                List<int> filledRows = _currentPieceController.CheckTileBelow(ref _shouldSpawnNewPiece);
                if (filledRows.Count > 0)
                {
                    _boardController.ClearCompletedLine(filledRows);
                    _scoreController.CleanLineAddScore(filledRows.Count);
                }

                userExecutingAction = false;

                _timer = _timeBetweenFalls;
                return;
            }

            _playerBehaviour.NeedToWaitForNextSpawn();
        }

        private void SpawnPiece()
        {
            _pieceSpawner.SpawnPiece(BoardConsts.REAL_ROWS, _boardController._board, (currentPiece, piece4x4SquareTiles, currentPieceTiles) =>
              {
                  _currentPieceController._currentPiece = currentPiece;
                  _currentPieceController._piece4x4CubeStartTile = piece4x4SquareTiles;
                  _currentPieceController._currentPieceTiles = currentPieceTiles;
              });
            _currentPieceController.OnSpawn();

        }

        public void HardDropPiece()
        {
            _currentPieceController.HardDropPiece();
        }

        public void MovePiecesInSomeDirection(int x, int y)
        {
            _currentPieceController.MovePiecesInSomeDirection(x, y);
        }

        public void RotatePiece(bool clockwise)
        {
            _currentPieceController.RotatePiece(clockwise);
        }
    }
}
