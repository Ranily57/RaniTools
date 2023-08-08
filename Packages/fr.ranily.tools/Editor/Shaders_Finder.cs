using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ShaderFinder : EditorWindow
{
    public Shader targetShader;

    private Vector2 scrollPos;
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("Tools/Ranily/Shader Finder")]
    static void Init()
    {
        ShaderFinder window = (ShaderFinder)EditorWindow.GetWindow(typeof(ShaderFinder));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Target Shader", EditorStyles.boldLabel);
        targetShader = (Shader)EditorGUILayout.ObjectField(targetShader, typeof(Shader), true);

        if (GUILayout.Button("Find GameObjects With Shader"))
        {
            FindObjectsWithShader();
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

    void FindObjectsWithShader()
    {
        foundObjects.Clear();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.sharedMaterials)
                {
                    if (mat.shader == targetShader)
                    {
                        foundObjects.Add(obj);
                        break;
                    }
                }
            }
        }
    }
}
