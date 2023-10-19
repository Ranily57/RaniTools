using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MaterialFinder : EditorWindow
{
    public Material targetMaterial;

    private Vector2 scrollPos;
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("Tools/Ranily/Material Finder")]
    static void Init()
    {
        MaterialFinder window = (MaterialFinder)EditorWindow.GetWindow(typeof(MaterialFinder));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Target Material", EditorStyles.boldLabel);
        targetMaterial = (Material)EditorGUILayout.ObjectField(targetMaterial, typeof(Material), true);

        if (GUILayout.Button("Find GameObjects With Material"))
        {
            FindObjectsWithMaterial();
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
}
