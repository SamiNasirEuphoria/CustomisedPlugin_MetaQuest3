using UnityEditor;

public class ModelImportSetting : AssetPostprocessor
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
    }
}