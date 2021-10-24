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

    private const int LEFT_PANEL_WIDTH = 120;
    private int _currentPiece = 0;
    private int _previousPiece = 0;

    public List<string> _piecesNames = new List<string>();
    public PiecesScriptable _currentPiecesScriptable;
    public Action<Piece> _onShowPieceForm;
    public Action _onChangePiece; 

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
        if(_currentPiecesScriptable == null)
        {
            string[] scriptableList = AssetDatabase.FindAssets("t:" + typeof(PiecesScriptable));
            if (scriptableList.Length == 0 || !AssetDatabase.GUIDToAssetPath(scriptableList[0]).Equals("Assets/Resources/Scriptables/PiecesScriptable.asset"))
            {
                string name = "Assets/Resources/Scriptables/PiecesScriptable.asset";
                PiecesScriptable asset = ScriptableObject.CreateInstance<PiecesScriptable>();
                AssetDatabase.CreateAsset(asset, name);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            _currentPiecesScriptable = (PiecesScriptable)EditorGUIUtility.Load("Assets/Resources/Scriptables/PiecesScriptable.asset");
        }

        if (_currentPiecesScriptable.pieces != null)
        {
            foreach (Piece piece in _currentPiecesScriptable.pieces)
            {
                _piecesNames.Add(piece.pieceName);
            }
        }

        PieceFormSelection.InitPieceFormSelection();
    }

    #endregion



    [MenuItem("Tetris/PiecesFormAndRotations")]
    public static void ShowWindow()
    {
        GetWindow<TetrisPiecesModifier>(false, "WindowTest", true);
    }


    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

            //Choose piece to modify
            GUILayout.BeginVertical(GUILayout.Width(10));
                EditorGUILayout.LabelField("Piece To Modify:", GUILayout.Width(LEFT_PANEL_WIDTH));
                _currentPiece = EditorGUILayout.Popup(_currentPiece, _piecesNames.ToArray(), GUILayout.Width(LEFT_PANEL_WIDTH));
                if (_currentPiece != _previousPiece)
                {
                    _onChangePiece?.Invoke();
                    _previousPiece = _currentPiece;
                }
            GUILayout.EndVertical();

            //Vertical separator
            EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(5), GUILayout.ExpandHeight(true));
            
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                _onShowPieceForm?.Invoke(_currentPiecesScriptable.pieces[_currentPiece]);
            GUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

         EditorUtility.SetDirty(_currentPiecesScriptable);
    }

}
