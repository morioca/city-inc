using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TitleScreen;

/// <summary>
/// Editor script to setup TitleScene with TitleSceneInitializer component.
/// </summary>
public class SetupTitleScene
{
    [MenuItem("Tools/Setup Title Scene")]
    public static void Setup()
    {
        var scenePath = "Assets/Scenes/TitleScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        var canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in TitleScene");
            return;
        }

        var existingInitializer = canvas.GetComponent<TitleSceneInitializer>();
        if (existingInitializer != null)
        {
            Debug.Log("TitleSceneInitializer already exists on Canvas");
            return;
        }

        var menuController = canvas.GetComponent<TitleMenuController>();
        if (menuController == null)
        {
            Debug.LogError("TitleMenuController not found on Canvas");
            return;
        }

        var initializer = canvas.AddComponent<TitleSceneInitializer>();

        var so = new SerializedObject(initializer);
        var menuControllerProp = so.FindProperty("<MenuController>k__BackingField");
        menuControllerProp.objectReferenceValue = menuController;
        so.ApplyModifiedProperties();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("TitleScene setup completed successfully");
    }
}
