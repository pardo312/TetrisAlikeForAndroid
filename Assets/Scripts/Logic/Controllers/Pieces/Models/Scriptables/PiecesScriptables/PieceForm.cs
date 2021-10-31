namespace JiufenGames.TetrisAlike.Model
{
    [System.Serializable]
    public class PieceForm
    {
        public string pieceFormName = "Need Name";

        //Indicate the form of the piece
        public const int PIECE_TILES_WIDTH = 4;

        //This is a flattened Array. This meas it was a bool[,] but was flattened to a bool[].
        //You need to call it like this pieceTiles(x+(y*WIDTH))
        public bool[] pieceTiles = new bool[4 * PIECE_TILES_WIDTH];
    }
}