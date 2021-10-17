using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JiufenGames.TetrisAlike.Logic
{
    public class PieceSpawner: MonoBehaviour
    {
        private Queue<Piece> _listOfNextPieces = new Queue<Piece>();
        [SerializeField] private PiecesScriptable _piecesTypes;
        public void Init()
        {
            _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);
        }

        public void SpawnPiece(int _realRows, Tile[,] _board, Action<Piece, Vector2Int, List<Vector2Int>> callback = null)
        {
            List<Vector2Int> currentPieceTiles = new List<Vector2Int>();
            Piece currentPiece = _listOfNextPieces.Dequeue();
            _listOfNextPieces.Enqueue(_piecesTypes.pieces[Random.Range(0, _piecesTypes.pieces.Length)]);

            int offset = 0;
            int highestOffset = 0;
            //Spawn Piece in the upper 4x4 space of the board
            for (int i = _realRows - 4; i < _realRows; i++)
            {
                bool rowFilled = false;
                for (int j = 3; j <= 6; j++)
                    if (_board[i, j]._isFilled)
                        rowFilled = true;

                if (rowFilled)
                    offset++;
            }

            Vector2Int piece4x4SquareTiles = new Vector2Int(_realRows - 4, 3);
            for (int i = _realRows - 4; i < _realRows; i++)
            {
                for (int j = 3; j <= 6; j++)
                    if (currentPiece.pieceForms[0].pieceTiles[((_realRows - 1) - i) + ((j - 3) * PieceForm.PIECE_TILES_WIDTH)])
                    {
                        if (offset > highestOffset)
                            piece4x4SquareTiles = new Vector2Int(i + offset - 4, 3);

                        _board[i + offset, j].ChangeColorOfTile(currentPiece.pieceColor);
                        currentPieceTiles.Add(new Vector2Int(i + offset, j));
                    }
            }
            callback?.Invoke(currentPiece, piece4x4SquareTiles, currentPieceTiles);
        }
    }
}
