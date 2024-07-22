using UnityEngine;
using UnityEditor;

public class PlatformSpecificImportSettings : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;

        // Check current build target
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            // Adjust import settings for Android
            modelImporter.globalScale = 1.00f;
            modelImporter.importNormals = ModelImporterNormals.Calculate;
            modelImporter.meshCompression = ModelImporterMeshCompression.Off;
            modelImporter.importCameras = false;
            modelImporter.importLights = false;
            // Add more Android-specific settings as needed
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            // Adjust import settings for iOS
            modelImporter.globalScale = 1.00f;
            modelImporter.importNormals = ModelImporterNormals.Calculate;
            modelImporter.meshCompression = ModelImporterMeshCompression.Off;
            modelImporter.importCameras = false;
            modelImporter.importLights = false;
        }

        Debug.Log("Model import settings applied to: " + assetPath);
    }

    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;

        //if (assetPath.StartsWith("Assets/0.Textures&Images"))
        //{
        //    // Adjust import settings for Android textures
        //    textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        //    textureImporter.maxTextureSize = 512;
        //    textureImporter.textureType = TextureImporterType.Sprite;
        //    textureImporter.mipmapEnabled = true;
        //    textureImporter.filterMode = FilterMode.Bilinear;

        //    TextureImporterPlatformSettings androidSettings = textureImporter.GetPlatformTextureSettings("Android");
        //    androidSettings.maxTextureSize = 512;
        //    androidSettings.format = TextureImporterFormat.ETC2_RGBA8Crunched;
        //    androidSettings.overridden = true;
        //    textureImporter.SetPlatformTextureSettings(androidSettings);
        //}
        // Check current build target
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            // Adjust import settings for Android textures
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            textureImporter.maxTextureSize = 2048;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.mipmapEnabled = true;
            textureImporter.filterMode = FilterMode.Trilinear;

            TextureImporterPlatformSettings androidSettings = textureImporter.GetPlatformTextureSettings("Android");
            androidSettings.maxTextureSize = 2048;
            androidSettings.format = TextureImporterFormat.ETC2_RGBA8Crunched;
            androidSettings.overridden = true;
            textureImporter.SetPlatformTextureSettings(androidSettings);

        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            // Adjust import settings for iOS textures
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            textureImporter.maxTextureSize = 2048;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.mipmapEnabled = true;
            textureImporter.filterMode = FilterMode.Trilinear;

            TextureImporterPlatformSettings iosSettings = textureImporter.GetPlatformTextureSettings("iPhone");
            iosSettings.maxTextureSize = 2048;
            iosSettings.format = TextureImporterFormat.ETC2_RGBA8Crunched;
            iosSettings.overridden = true;
            textureImporter.SetPlatformTextureSettings(iosSettings);
        }

        Debug.Log("Texture import settings applied to: " + assetPath+ "and the type is "+textureImporter.textureType);
    }
}
