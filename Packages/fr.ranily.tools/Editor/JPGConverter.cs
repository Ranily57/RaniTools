using UnityEngine;
using UnityEditor;
using System.Linq;

public class TextureConverterTool : EditorWindow
{
    private Texture2D pngTexture;

    [MenuItem("Tools/Ranily/Texture Converter")]
    private static void ShowWindow()
    {
        GetWindow<TextureConverterTool>("Texture Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Insérez la texture PNG à convertir en JPG", EditorStyles.boldLabel);
        pngTexture = EditorGUILayout.ObjectField("Texture PNG", pngTexture, typeof(Texture2D), false) as Texture2D;

        if(GUILayout.Button("Convertir et remplacer dans les matériaux"))
        {
            ConvertAndReplace();
        }
    }

private void ConvertAndReplace()
{
    if(pngTexture == null)
    {
        Debug.LogError("Aucune texture PNG spécifiée!");
        return;
    }

    string pngAssetPath = AssetDatabase.GetAssetPath(pngTexture);

    // Get the TextureImporter for the texture
    TextureImporter textureImporter = AssetImporter.GetAtPath(pngAssetPath) as TextureImporter;

    if (textureImporter == null)
    {
        Debug.LogError("TextureImporter n'a pas pu être obtenu pour la texture.");
        return;
    }

    // Save the original import settings
    bool originalIsReadable = textureImporter.isReadable;
    TextureImporterCompression originalCompression = textureImporter.textureCompression;
    int originalMaxTextureSize = textureImporter.maxTextureSize;

    // Make the texture readable and uncompressed
    textureImporter.isReadable = true;
    textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
    textureImporter.maxTextureSize = 4096;

    AssetDatabase.ImportAsset(pngAssetPath, ImportAssetOptions.ForceUpdate);

    Texture2D newTexture = new Texture2D(pngTexture.width, pngTexture.height);
    newTexture.SetPixels(pngTexture.GetPixels());
    newTexture.Apply();

    byte[] jpgBytes = newTexture.EncodeToJPG();
    string jpgPath = pngAssetPath.Replace(".png", ".jpg");
    System.IO.File.WriteAllBytes(jpgPath, jpgBytes);

    // Refresh the AssetDatabase after creating the new texture
    AssetDatabase.Refresh();

    Texture2D jpgTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(jpgPath);

    var allMaterials = Resources.FindObjectsOfTypeAll<Material>();

    foreach (var material in allMaterials)
    {
        // Iterate over each texture property in the material
        foreach (string texturePropertyName in material.GetTexturePropertyNames())
        {
            if (material.GetTexture(texturePropertyName) == pngTexture)
            {
                material.SetTexture(texturePropertyName, jpgTexture);
            }
        }
    }

    // Restore the original import settings
    textureImporter.isReadable = originalIsReadable;
    textureImporter.textureCompression = originalCompression;
    textureImporter.maxTextureSize = originalMaxTextureSize;

    AssetDatabase.ImportAsset(pngAssetPath, ImportAssetOptions.ForceUpdate);
}



}
