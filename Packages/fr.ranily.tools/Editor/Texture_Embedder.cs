#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class Texture_Embedder : EditorWindow
{
    private GameObject model;
    private string textureFolderPath;
    private Material[] materials;

    [MenuItem("Window/Texture Assigner")]
    public static void ShowWindow()
    {
        GetWindow<Texture_Embedder>("Texture Assigner");
    }

    void OnGUI()
    {
        GUILayout.Label("Assign Textures to Model", EditorStyles.boldLabel);

        model = EditorGUILayout.ObjectField("Model", model, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Select Texture Folder"))
        {
            textureFolderPath = EditorUtility.OpenFolderPanel("Select Texture Folder", "", "");

            if (!string.IsNullOrEmpty(textureFolderPath))
            {
                string relativePath = "Assets" + textureFolderPath.Substring(Application.dataPath.Length);
                textureFolderPath = relativePath;
            }
        }

        EditorGUILayout.LabelField("Texture Folder", textureFolderPath);

        if (model != null && !string.IsNullOrEmpty(textureFolderPath))
        {
            if (GUILayout.Button("Assign Textures"))
            {
                AssignTextures();
            }
        }
    }

    void AssignTextures()
    {
        if (model == null || string.IsNullOrEmpty(textureFolderPath))
        {
            Debug.LogError("Model or Texture Folder is not selected.");
            return;
        }

        materials = model.GetComponent<Renderer>().sharedMaterials;

        foreach (var material in materials)
        {
            string textureName = material.name.Replace(" (Instance)", "");
            string texturePath = Path.Combine(textureFolderPath, textureName + ".tga");

            if (File.Exists(texturePath))
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                material.mainTexture = texture;
            }
            else
            {
                Debug.LogWarning("Texture not found: " + texturePath);
            }
        }
    }
}
#endif