using JiufenGames.TetrisAlike.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Tile[,] Board;
        [SerializeField] private Tile TilePrefab;
        [SerializeField] private Piece[] PieceTypes;
        [SerializeField] private Queue<Piece> ListOfNextPieces;
        private Piece CurrentPiece;
        private int ROWS = 12;
        private int COLUMNS = 6;

        void Start()
        {
            Board = new Tile[ROWS, COLUMNS];
            for (int i = 0; i < ROWS; i++)
                for (int j = 0; j < COLUMNS; j++)
                    Board[i, j] = Instantiate(TilePrefab, new Vector3(i + 1, j + 1, 0), Quaternion.identity);

            for (int k = 0; k < PieceTypes.Length - 1; k++)
                ListOfNextPieces.Enqueue(PieceTypes[Random.Range(0, PieceTypes.Length)]);
        }

        public bool shouldSpawnNewPiece = true;
        void Update()
        {
            if (!shouldSpawnNewPiece)
                return;
            if (CurrentPiece == null)
            {
                CurrentPiece = ListOfNextPieces.Dequeue();
                ListOfNextPieces.Enqueue(PieceTypes[Random.Range(0, PieceTypes.Length)]);
            }


        }
    }
}
