using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Adds required scenes to build settings.
/// </summary>
[InitializeOnLoad]
public class AddScenesToBuildSettings
{
    static AddScenesToBuildSettings()
    {
        var requiredScenes = new[]
        {
            "Assets/Scenes/TitleScene.unity",
            "Assets/Scenes/MainGameScene.unity"
        };

        var currentScenes = EditorBuildSettings.scenes.ToList();
        var scenePathsInBuild = new HashSet<string>(currentScenes.Select(s => s.path));
        var modified = false;

        foreach (var scenePath in requiredScenes)
        {
            if (!scenePathsInBuild.Contains(scenePath))
            {
                var guid = AssetDatabase.AssetPathToGUID(scenePath);
                if (!string.IsNullOrEmpty(guid))
                {
                    currentScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    modified = true;
                    UnityEngine.Debug.Log($"Added {scenePath} to build settings");
                }
            }
        }

        if (modified)
        {
            EditorBuildSettings.scenes = currentScenes.ToArray();
        }
    }
}
