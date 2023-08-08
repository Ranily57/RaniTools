using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextureFinder : EditorWindow
{
    public Texture2D targetTexture;

    private Vector2 scrollPos;
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("Tools/Ranily/Texture Finder")]
    static void Init()
    {
        TextureFinder window = (TextureFinder)EditorWindow.GetWindow(typeof(TextureFinder));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Target Texture", EditorStyles.boldLabel);
        targetTexture = (Texture2D)EditorGUILayout.ObjectField(targetTexture, typeof(Texture2D), true);

        if (GUILayout.Button("Find GameObjects With Texture"))
        {
            FindObjectsWithTexture();
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
}
