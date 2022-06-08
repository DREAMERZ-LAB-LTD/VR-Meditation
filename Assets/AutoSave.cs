#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

/// <summary>
/// This Script will help to save the scene change if the unity crash after hit run.
/// </summary>
[InitializeOnLoad]
public class AutoSave
{
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto-saving...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}
#endif