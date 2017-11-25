using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    LevelGenerator script;
    GameObject hierarchy;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    
        script = target as LevelGenerator;

        if (GUILayout.Button( "Generate"))
        {
            script.GenerateLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            EditorSceneManager.OpenScene("Assets/_Scenes/NO DEFINITIVO/LevelGenerator.unity");
        }

        if (GUILayout.Button("Save As Prefab"))
        {
            hierarchy = GameObject.FindGameObjectWithTag("Environment");

            if ((AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Levels/" + script.prefabName + ".prefab",typeof(object)) == null))
            {
                PrefabUtility.CreatePrefab("Assets/Prefabs/Levels/" + script.prefabName + ".prefab", hierarchy);
            }
            else
            {
                EditorApplication.Beep();
                Debug.LogError("There's already a prefab with that name in the directory, please choose another name. ");
            }
        }
    }

}
