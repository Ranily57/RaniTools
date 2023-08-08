using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FinderTool : EditorWindow
{
    public Texture2D targetTexture;
    public Material targetMaterial;
    public MonoScript targetScript;
    public Shader targetShader;

    private Vector2 scrollPos;
    private List<GameObject> foundObjects = new List<GameObject>();

    private int tab;

    [MenuItem("Tools/Ranily/Finder Tool")]
    static void Init()
    {
        FinderTool window = (FinderTool)EditorWindow.GetWindow(typeof(FinderTool));
        window.Show();
    }

    void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new string[] { "Texture Finder", "Material Finder", "Script Finder", "Shader Finder" });

        switch (tab)
        {
            case 0:
                GUILayout.Label("Target Texture", EditorStyles.boldLabel);
                targetTexture = (Texture2D)EditorGUILayout.ObjectField(targetTexture, typeof(Texture2D), true);
                if (GUILayout.Button("Find GameObjects With Texture"))
                {
                    FindObjectsWithTexture();
                }
                break;
            case 1:
                GUILayout.Label("Target Material", EditorStyles.boldLabel);
                targetMaterial = (Material)EditorGUILayout.ObjectField(targetMaterial, typeof(Material), true);
                if (GUILayout.Button("Find GameObjects With Material"))
                {
                    FindObjectsWithMaterial();
                }
                break;
            case 2:
                GUILayout.Label("Target Script", EditorStyles.boldLabel);
                targetScript = (MonoScript)EditorGUILayout.ObjectField(targetScript, typeof(MonoScript), true);
                if (GUILayout.Button("Find GameObjects With Script"))
                {
                    FindObjectsWithScript();
                }
                break;
            case 3:
                GUILayout.Label("Target Shader", EditorStyles.boldLabel);
                targetShader = (Shader)EditorGUILayout.ObjectField(targetShader, typeof(Shader), true);
                if (GUILayout.Button("Find GameObjects With Shader"))
                {
                    FindObjectsWithShader();
                }
                break;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach (GameObject obj in foundObjects)
        {
            if (GUILayout.Button(obj.name))
            {
                Selection.activeGameObject = obj;
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void FindObjectsWithTexture()
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
                    if (mat.mainTexture == targetTexture)
                    {
                        foundObjects.Add(obj);
                        break;
                    }
                }
            }
        }
    }

    void FindObjectsWithMaterial()
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
                    if (mat == targetMaterial)
                    {
                        foundObjects.Add(obj);
                        break;
                    }
                }
            }
        }
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
