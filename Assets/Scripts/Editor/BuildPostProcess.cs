using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Reporting;

public class BuildPostProcessor: IPostprocessBuildWithReport
{
    public int callbackOrder => 0;



    public void OnPostprocessBuild(BuildReport report)
    {
        // Define the source and destination paths
        string sourceFilePath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");
        string destinationFilePath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");

        // Check if the source file exists
        if (File.Exists(sourceFilePath))
        {
            // Create the destination folder if it does not exist
            string destinationFolder = Path.GetDirectoryName(destinationFilePath);
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Move the file
            File.Move(sourceFilePath, destinationFilePath);
            UnityEngine.Debug.Log($"File moved from {sourceFilePath} to {destinationFilePath}");
        }
        else
        {
            UnityEngine.Debug.LogError($"Source file does not exist at {sourceFilePath}");
        }
    }



    //[MenuItem("Build/Perform Post Build Actions")]
    //public static void PerformPostBuildActions()
    //{
    //    // Check if the build target is Android
    //    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
    //    {
    //        // Define paths
    //        // Note: You might need to change these paths based on your actual setup
    //        string editorDataPath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");
    //        string buildDataPath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");

    //        // Ensure the file exists in the editor's StreamingAssets path
    //        if (File.Exists(editorDataPath))
    //        {
    //            // Move the file to the build's StreamingAssets path (assuming the build is accessible in the same directory)
    //            File.Copy(editorDataPath, buildDataPath, true);
    //            Debug.Log("File moved successfully.");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("File not found in the editor's StreamingAssets path.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Current build target is not Android. Skipping file operation.");
    //    }
    //}
}
