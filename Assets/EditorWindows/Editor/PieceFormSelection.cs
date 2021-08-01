
using System;
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
            TetrisPiecesModifier.Instance.OnShowPieceForm += ShowPieceForm;
        }
        public static List<string> PieceFormOptions = new List<string>();
        public static int currentPieceForm = 0;
        public static int previousPieceForm = 0;

        public static void ShowPieceForm(Piece currentPiece)
        {
            PieceFormOptions = new List<string>();
            foreach (PieceForm pieceForm in currentPiece.PieceForms)
            {
                PieceFormOptions.Add(pieceForm.pieceFormName);
            }

            EditorGUILayout.LabelField("PieceForm:", GUILayout.ExpandWidth(true));
            currentPieceForm = EditorGUILayout.Popup(currentPieceForm, PieceFormOptions.ToArray(), GUILayout.ExpandWidth(true));

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.ExpandWidth(true));
            currentPiece.PieceColor = EditorGUILayout.ColorField(currentPiece.PieceColor,  GUILayout.ExpandWidth(true));

            //Slider
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.ExpandWidth(true));
            bool[,] currentPieceTiles = currentPiece.PieceForms[currentPieceForm].PieceTiles;
            ShowPieceFormEditor(currentPieceTiles,currentPiece.PieceColor);

        }

        private static void ShowPieceFormEditor(bool[,] tiles, Color currentPieceColor)
        {
            //Style Of The row column button
            GUI.backgroundColor = Color.white;
            for (int i = 0; i < 4; i++)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
                    GUILayout.FlexibleSpace();
                    for (int j = 0; j < 4; j++)
                    {
                        PaintButton(i,j,tiles,currentPieceColor);
                    }
                    GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.FlexibleSpace();

        }

        private static void PaintButton(int i, int j,bool[,] tiles, Color currentPieceColor)
        {
            if (tiles[i,j])
            {
                GUI.backgroundColor = currentPieceColor;
                if (GUILayout.Button("", GUILayout.Width(80), GUILayout.Height(80)))
                {
                    tiles[i, j] = false;
                }
                GUI.backgroundColor = Color.white;
            }
            else
            {
                if (GUILayout.Button("-", GUILayout.Width(80), GUILayout.Height(80)))
                {
                    tiles[i, j] = true;
                }
            }

        }
    }
}
