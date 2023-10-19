using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ScriptFinder : EditorWindow
{
    public MonoScript targetScript;

    private Vector2 scrollPos;
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("Tools/Ranily/Script Finder")]
    static void Init()
    {
        ScriptFinder window = (ScriptFinder)EditorWindow.GetWindow(typeof(ScriptFinder));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Target Script", EditorStyles.boldLabel);
        targetScript = (MonoScript)EditorGUILayout.ObjectField(targetScript, typeof(MonoScript), true);

        if (GUILayout.Button("Find GameObjects With Script"))
        {
            FindObjectsWithScript();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach(GameObject obj in foundObjects)
        {
            if (GUILayout.Button(obj.name))
            {
                Selection.activeGameObject = obj;
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void FindObjectsWithScript()
    {
        foundObjects.Clear();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().ToString() == targetScript.GetClass().ToString())
                {
                    foundObjects.Add(obj);
                    break;
                }
            }
        }
    }
}
