using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Unity UI Text components to TextMesh Pro converter
/// </summary>
public static class ConvertToTextMeshPro
{
    /// <summary>
    /// Convert all UI Text components in the active scene to TextMeshProUGUI
    /// </summary>
    [MenuItem("Tools/Convert UI Text to TextMesh Pro")]
    public static void ConvertAllTextInScene()
    {
        // Find Noto Sans JP SDF font
        TMP_FontAsset notoSansJpFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/TextMesh Pro/Fonts/NotoSansJP SDF.asset");

        if (notoSansJpFont == null)
        {
            Debug.LogError("NotoSansJP SDF.asset not found. Please create the font asset first.");
            return;
        }

        // Find all Text components in the scene
        Text[] textComponents = Object.FindObjectsOfType<Text>(true);

        if (textComponents.Length == 0)
        {
            Debug.Log("No UI Text components found in the scene.");
            return;
        }

        int convertedCount = 0;

        foreach (Text text in textComponents)
        {
            GameObject gameObject = text.gameObject;

            // Store original settings
            string originalText = text.text;
            Color originalColor = text.color;
            int originalFontSize = text.fontSize;
            TextAnchor originalAlignment = text.alignment;
            bool originalRaycastTarget = text.raycastTarget;

            // Remove the old Text component
            Object.DestroyImmediate(text);

            // Add TextMeshProUGUI component
            TextMeshProUGUI tmpText = gameObject.AddComponent<TextMeshProUGUI>();

            // Apply settings
            tmpText.text = originalText;
            tmpText.color = originalColor;
            tmpText.fontSize = originalFontSize;
            tmpText.alignment = ConvertAlignment(originalAlignment);
            tmpText.raycastTarget = originalRaycastTarget;
            tmpText.font = notoSansJpFont;

            Debug.Log($"Converted: {gameObject.name}");
            convertedCount++;
        }

        Debug.Log($"Conversion complete: {convertedCount} Text components converted to TextMeshProUGUI.");
        EditorUtility.DisplayDialog(
            "Conversion Complete",
            $"{convertedCount} UI Text components have been converted to TextMesh Pro.\n\n" +
            "Font Asset: NotoSansJP SDF",
            "OK");
    }

    /// <summary>
    /// Convert TextAnchor to TextAlignmentOptions
    /// </summary>
    private static TextAlignmentOptions ConvertAlignment(TextAnchor anchor)
    {
        return anchor switch
        {
            TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
            TextAnchor.UpperCenter => TextAlignmentOptions.Top,
            TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
            TextAnchor.MiddleLeft => TextAlignmentOptions.Left,
            TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
            TextAnchor.MiddleRight => TextAlignmentOptions.Right,
            TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
            TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
            TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
            _ => TextAlignmentOptions.Center,
        };
    }
}
