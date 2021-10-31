using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Model
{
    public class PieceFormSelection : EditorWindow
    {
        private const int LEFT_PANEL_WIDTH = 120;

        public static void InitPieceFormSelection()
        {
            TetrisPiecesModifier.Instance._onShowPieceForm += ShowPieceForm;
            TetrisPiecesModifier.Instance._onChangePiece += () => { currentPieceForm = 0; previousPieceForm = 0; };
        }

        public static List<string> PieceFormOptions = new List<string>();
        public static int currentPieceForm = 0;
        public static int previousPieceForm = 0;

        public static void ShowPieceForm(Piece currentPiece)
        {
            PieceFormOptions = new List<string>();
            foreach (PieceForm pieceForm in currentPiece.pieceForms)
            {
                PieceFormOptions.Add(pieceForm.pieceFormName);
            }

            EditorGUILayout.LabelField("PieceForm:", GUILayout.ExpandWidth(true));
            currentPieceForm = EditorGUILayout.Popup(currentPieceForm, PieceFormOptions.ToArray(), GUILayout.ExpandWidth(true));

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.ExpandWidth(true));
            currentPiece.pieceColor = EditorGUILayout.ColorField(currentPiece.pieceColor, GUILayout.ExpandWidth(true));
            //ShowListOfPieceTiles(currentPiece, currentPieceForm);
            //Slider
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.ExpandWidth(true));
            ShowPieceFormEditor(currentPiece.pieceForms[currentPieceForm].pieceTiles, currentPiece.pieceColor);
        }

        private static void ShowPieceFormEditor(bool[] tiles, Color currentPieceColor)
        {
            //Style Of The row column button
            GUI.backgroundColor = Color.white;
            for (int i = 3; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
                GUILayout.FlexibleSpace();
                for (int j = 0; j < 4; j++)
                {
                    PaintButton(i, j, tiles, currentPieceColor);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.FlexibleSpace();
        }

        private static void PaintButton(int i, int j, bool[] tiles, Color currentPieceColor)
        {
            if (tiles[i + (j * PieceForm.PIECE_TILES_WIDTH)])
            {
                GUI.backgroundColor = currentPieceColor;
                if (GUILayout.Button("", GUILayout.Width(80), GUILayout.Height(80)))
                {
                    tiles[i + (j * PieceForm.PIECE_TILES_WIDTH)] = false;
                }
                GUI.backgroundColor = Color.white;
            }
            else
            {
                if (GUILayout.Button("-", GUILayout.Width(80), GUILayout.Height(80)))
                {
                    tiles[i + (j * PieceForm.PIECE_TILES_WIDTH)] = true;
                }
            }
        }
    }
}