using JiufenGames.TetrisAlike.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TetrisPiecesModifier : EditorWindow
{
    #region Singleton And Events
    private static TetrisPiecesModifier _instance;

    public static TetrisPiecesModifier Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        string[] scriptableList = AssetDatabase.FindAssets("t:" + typeof(PiecesScriptable));
        if (scriptableList.Length == 0 || !AssetDatabase.GUIDToAssetPath(scriptableList[0]).Equals("Assets/Resources/Scriptables/PiecesScriptable.asset"))
        {
            string name = "Assets/Resources/Scriptables/PiecesScriptable.asset";
            PiecesScriptable asset = ScriptableObject.CreateInstance<PiecesScriptable>();
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        PiecesScriptable = (PiecesScriptable)EditorGUIUtility.Load("Assets/Resources/Scriptables/PiecesScriptable.asset");
        if (PiecesScriptable.Pieces != null)
        {
            foreach (Piece piece in PiecesScriptable.Pieces)
            {
                PiecesNames.Add(piece.PieceName);
            }
        }

        PieceFormSelection.InitPieceFormSelection();
    }

    #endregion

    public List<string> PiecesNames = new List<string>();
    public PiecesScriptable PiecesScriptable;
    private const int LEFT_PANEL_WIDTH = 120;


    [MenuItem("Geta/Platform Models Tests")]
    public static void ShowWindow()
    {
        GetWindow<TetrisPiecesModifier>(false, "WindowTest", true);
    }

    private int currentPiece = 0;
    private int previousPiece = 0;
    public Action<Piece> OnShowPieceForm;

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

            //Choose piece to modify
            GUILayout.BeginVertical(GUILayout.Width(10));
                EditorGUILayout.LabelField("Piece To Modify:", GUILayout.Width(LEFT_PANEL_WIDTH));
                currentPiece = EditorGUILayout.Popup(currentPiece, PiecesNames.ToArray(), GUILayout.Width(LEFT_PANEL_WIDTH));
                if (currentPiece != previousPiece)
                {
                    previousPiece = currentPiece;
                }
            GUILayout.EndVertical();

            //Vertical separator
            EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(5), GUILayout.ExpandHeight(true));
            
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                OnShowPieceForm?.Invoke(PiecesScriptable.Pieces[currentPiece]);
            GUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

}
