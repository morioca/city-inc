using Domain.Models;
using Presentation;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// Editor utility to set up the MainGame scene with UI elements.
    /// </summary>
    public static class MainGameSceneSetup
    {
        [MenuItem("Tools/Setup MainGame Scene")]
        public static void SetupScene()
        {
            var scenePath = "Assets/Scenes/MainGameScene.unity";
            var scene = EditorSceneManager.OpenScene(scenePath);

            var canvas = CreateCanvas();
            var dateLabel = CreateDateLabel(canvas.transform);
            var nextMonthButton = CreateNextMonthButton(canvas.transform);
            var presenter = CreateGameStatePresenter(canvas.transform, dateLabel, nextMonthButton);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log("MainGameScene setup completed successfully.");
        }

        private static GameObject CreateCanvas()
        {
            var canvasObject = new GameObject("Canvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
            canvasScaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();

            return canvasObject;
        }

        private static TMP_Text CreateDateLabel(Transform parent)
        {
            var dateLabelObject = new GameObject("DateLabel");
            dateLabelObject.transform.SetParent(parent, false);

            var rectTransform = dateLabelObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -50);
            rectTransform.sizeDelta = new Vector2(600, 100);

            var textComponent = dateLabelObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "2024年04月";
            textComponent.fontSize = 48;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return textComponent;
        }

        private static Button CreateNextMonthButton(Transform parent)
        {
            var buttonObject = new GameObject("NextMonthButton");
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0, 100);
            rectTransform.sizeDelta = new Vector2(300, 80);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.2f, 0.6f, 0.9f, 1f);

            var button = buttonObject.AddComponent<Button>();

            var textObject = new GameObject("Text");
            textObject.transform.SetParent(buttonObject.transform, false);

            var textRectTransform = textObject.AddComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            var textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "次の月へ";
            textComponent.fontSize = 32;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return button;
        }

        private static GameStatePresenter CreateGameStatePresenter(Transform parent, TMP_Text dateLabel,
            Button nextMonthButton)
        {
            var presenterObject = new GameObject("GameStatePresenter");
            presenterObject.transform.SetParent(parent, false);

            var presenter = presenterObject.AddComponent<GameStatePresenter>();

            var so = new SerializedObject(presenter);
            so.FindProperty("<DateLabel>k__BackingField").objectReferenceValue = dateLabel;
            so.FindProperty("<NextMonthButton>k__BackingField").objectReferenceValue = nextMonthButton;
            so.ApplyModifiedProperties();

            nextMonthButton.onClick.AddListener(presenter.OnNextMonthButtonClicked);

            var initialState = new GameState(new GameDate(2024, 4));
            presenter.Initialize(initialState);

            return presenter;
        }
    }
}
