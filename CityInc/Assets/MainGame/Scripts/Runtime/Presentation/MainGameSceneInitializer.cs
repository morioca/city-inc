using Domain.Models;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Presentation
{
    /// <summary>
    /// Initializes the MainGame scene by creating UI elements at runtime.
    /// </summary>
    public class MainGameSceneInitializer : MonoBehaviour
    {
        private void Start()
        {
            SetupScene();
        }

        private void SetupScene()
        {
            var canvas = CreateCanvas();
            CreateEventSystem();
            var safeAreaPanel = CreateSafeAreaPanel(canvas.transform);
            var statusBarPanel = CreateStatusBarPanel(safeAreaPanel.transform);
            var populationLabel = CreatePopulationLabel(statusBarPanel.transform);
            var budgetLabel = CreateBudgetLabel(statusBarPanel.transform);
            var approvalRatingLabel = CreateApprovalRatingLabel(statusBarPanel.transform);
            var dateLabel = CreateDateLabel(safeAreaPanel.transform);
            var nextMonthButton = CreateNextMonthButton(safeAreaPanel.transform);
            CreateGameStatePresenter(safeAreaPanel.transform, dateLabel, populationLabel, budgetLabel, approvalRatingLabel, nextMonthButton);
        }

        private GameObject CreateCanvas()
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

        private void CreateEventSystem()
        {
            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        private GameObject CreateSafeAreaPanel(Transform parent)
        {
            var panelObject = new GameObject("SafeAreaPanel");
            panelObject.transform.SetParent(parent, false);

            var rectTransform = panelObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            panelObject.AddComponent<SafeAreaLayout>();

            return panelObject;
        }

        private GameObject CreateStatusBarPanel(Transform parent)
        {
            var panelObject = new GameObject("StatusBarPanel");
            panelObject.transform.SetParent(parent, false);

            var rectTransform = panelObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(0, 100);

            var layoutGroup = panelObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.spacing = 20;
            layoutGroup.padding = new RectOffset(20, 20, 10, 10);

            return panelObject;
        }

        private TMP_Text CreatePopulationLabel(Transform parent)
        {
            var labelObject = new GameObject("PopulationLabel");
            labelObject.transform.SetParent(parent, false);

            var textComponent = labelObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "人口 50,000人";
            textComponent.fontSize = 24;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return textComponent;
        }

        private TMP_Text CreateBudgetLabel(Transform parent)
        {
            var labelObject = new GameObject("BudgetLabel");
            labelObject.transform.SetParent(parent, false);

            var textComponent = labelObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "財政 100,000,000円";
            textComponent.fontSize = 24;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return textComponent;
        }

        private TMP_Text CreateApprovalRatingLabel(Transform parent)
        {
            var labelObject = new GameObject("ApprovalRatingLabel");
            labelObject.transform.SetParent(parent, false);

            var textComponent = labelObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "支持率 60%";
            textComponent.fontSize = 24;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return textComponent;
        }

        private TMP_Text CreateDateLabel(Transform parent)
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

        private Button CreateNextMonthButton(Transform parent)
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

        private void CreateGameStatePresenter(Transform parent, TMP_Text dateLabel, TMP_Text populationLabel, TMP_Text budgetLabel, TMP_Text approvalRatingLabel, Button nextMonthButton)
        {
            var presenterObject = new GameObject("GameStatePresenter");
            presenterObject.transform.SetParent(parent, false);

            var presenter = presenterObject.AddComponent<GameStatePresenter>();

            var dateField = typeof(GameStatePresenter).GetField("<DateLabel>k__BackingField",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            dateField?.SetValue(presenter, dateLabel);

            var populationField = typeof(GameStatePresenter).GetField("<PopulationLabel>k__BackingField",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            populationField?.SetValue(presenter, populationLabel);

            var budgetField = typeof(GameStatePresenter).GetField("<BudgetLabel>k__BackingField",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            budgetField?.SetValue(presenter, budgetLabel);

            var approvalRatingField = typeof(GameStatePresenter).GetField("<ApprovalRatingLabel>k__BackingField",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            approvalRatingField?.SetValue(presenter, approvalRatingLabel);

            var buttonField = typeof(GameStatePresenter).GetField("<NextMonthButton>k__BackingField",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            buttonField?.SetValue(presenter, nextMonthButton);

            nextMonthButton.onClick.AddListener(presenter.OnNextMonthButtonClicked);

            var initialState = GameState.CreateInitial();
            presenter.Initialize(initialState);
        }
    }
}
