using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PiecePrefabMapping))]
public class PiecePrefabMappingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PiecePrefabMapping mapping = (PiecePrefabMapping)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Piece Prefab Mappings");

        if (mapping.mappings != null)
        {
            foreach (var entry in mapping.mappings)
            {
                EditorGUILayout.BeginHorizontal();
                entry.pieceType = (PieceType)EditorGUILayout.EnumPopup(entry.pieceType);
                entry.playerPrefab = (GameObject)EditorGUILayout.ObjectField(entry.playerPrefab, typeof(GameObject), false);
                entry.opponentPrefab = (GameObject)EditorGUILayout.ObjectField(entry.opponentPrefab, typeof(GameObject), false);
                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Add Mapping"))
        {
            ArrayUtility.Add(ref mapping.mappings, new PiecePrefabMappingEntry());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}