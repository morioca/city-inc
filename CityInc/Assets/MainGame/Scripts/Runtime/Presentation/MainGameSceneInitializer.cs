using Domain.Models;
using Domain.Systems;
using Presentation.Budget;
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
        private GameStatePresenter _gameStatePresenter;
        private BudgetAllocationPresenter _budgetPresenter;
        private GameObject _budgetScreen;
        private readonly BudgetValidator _budgetValidator = new BudgetValidator();

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
            var budgetButton = CreateBudgetButton(safeAreaPanel.transform);
            var dateLabel = CreateDateLabel(safeAreaPanel.transform);
            var nextMonthButton = CreateNextMonthButton(safeAreaPanel.transform);
            _gameStatePresenter = CreateGameStatePresenter(safeAreaPanel.transform, dateLabel, populationLabel, budgetLabel, approvalRatingLabel, nextMonthButton);
            _budgetScreen = CreateBudgetScreen(canvas.transform);

            budgetButton.onClick.AddListener(OnBudgetButtonClicked);
        }

        private void OnBudgetButtonClicked()
        {
            _budgetScreen.SetActive(true);
            _budgetPresenter.Initialize(_gameStatePresenter.CurrentState, _budgetValidator);
        }

        private void OnAllocationConfirmed(BudgetAllocation allocation)
        {
            _gameStatePresenter.UpdateAllocation(allocation);
        }

        private void OnModalClosed()
        {
            _budgetScreen.SetActive(false);
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
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.spacing = 20;
            layoutGroup.padding = new RectOffset(20, 20, 10, 10);

            return panelObject;
        }

        private TMP_Text CreatePopulationLabel(Transform parent)
        {
            var labelObject = new GameObject("PopulationLabel");
            labelObject.transform.SetParent(parent, false);

            var rectTransform = labelObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);

            var layoutElement = labelObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 200;
            layoutElement.preferredHeight = 50;

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

            var rectTransform = labelObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(250, 50);

            var layoutElement = labelObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 250;
            layoutElement.preferredHeight = 50;

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

            var rectTransform = labelObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 50);

            var layoutElement = labelObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 150;
            layoutElement.preferredHeight = 50;

            var textComponent = labelObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "支持率 60%";
            textComponent.fontSize = 24;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return textComponent;
        }

        private Button CreateBudgetButton(Transform parent)
        {
            var buttonObject = new GameObject("BudgetButton");
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -110);
            rectTransform.sizeDelta = new Vector2(0, 60);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.15f, 0.15f, 0.3f, 1f);

            var button = buttonObject.AddComponent<Button>();

            var textObject = new GameObject("Text");
            textObject.transform.SetParent(buttonObject.transform, false);

            var textRectTransform = textObject.AddComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            var textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = "Budget";
            textComponent.fontSize = 28;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return button;
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

        private GameStatePresenter CreateGameStatePresenter(Transform parent, TMP_Text dateLabel, TMP_Text populationLabel, TMP_Text budgetLabel, TMP_Text approvalRatingLabel, Button nextMonthButton)
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

            return presenter;
        }

        private GameObject CreateBudgetScreen(Transform parent)
        {
            var screenObject = new GameObject("BudgetScreen");
            screenObject.transform.SetParent(parent, false);

            var rectTransform = screenObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            var backgroundImage = screenObject.AddComponent<Image>();
            backgroundImage.color = new Color(0.95f, 0.95f, 0.92f, 1f);

            _budgetPresenter = screenObject.AddComponent<BudgetAllocationPresenter>();

            var totalBudgetLabel = CreateBudgetScreenLabel(screenObject.transform, "TotalBudgetLabel", 24);
            var remainingBudgetLabel = CreateBudgetScreenLabel(screenObject.transform, "RemainingBudgetLabel", 20);

            SetSerializedField(_budgetPresenter, "TotalBudgetLabel", totalBudgetLabel);
            SetSerializedField(_budgetPresenter, "RemainingBudgetLabel", remainingBudgetLabel);

            CreateCategoryRow(screenObject.transform, "Welfare", "福祉・医療");
            CreateCategoryRow(screenObject.transform, "Education", "教育・子育て");
            CreateCategoryRow(screenObject.transform, "Industry", "産業振興");
            CreateCategoryRow(screenObject.transform, "Infrastructure", "インフラ整備");
            CreateCategoryRow(screenObject.transform, "Disaster", "防災・安全");
            CreateCategoryRow(screenObject.transform, "Tourism", "観光・文化");

            var confirmButton = CreateBudgetScreenButton(screenObject.transform, "ConfirmButton", "Confirm");
            var closeButton = CreateBudgetScreenButton(screenObject.transform, "CloseButton", "Close");

            SetSerializedField(_budgetPresenter, "ConfirmButton", confirmButton);
            SetSerializedField(_budgetPresenter, "CloseButton", closeButton);

            confirmButton.onClick.AddListener(_budgetPresenter.OnConfirmButtonClicked);
            closeButton.onClick.AddListener(_budgetPresenter.OnCloseButtonClicked);

            _budgetPresenter.AllocationConfirmed += OnAllocationConfirmed;
            _budgetPresenter.ModalClosed += OnModalClosed;

            screenObject.SetActive(false);

            return screenObject;
        }

        private void CreateCategoryRow(Transform parent, string category, string displayName)
        {
            var rowObject = new GameObject(category + "Row");
            rowObject.transform.SetParent(parent, false);
            rowObject.AddComponent<RectTransform>();

            var nameLabel = new GameObject(category + "NameLabel");
            nameLabel.transform.SetParent(rowObject.transform, false);
            nameLabel.AddComponent<RectTransform>();
            var nameText = nameLabel.AddComponent<TextMeshProUGUI>();
            nameText.text = displayName;
            nameText.fontSize = 18;

            var sliderObject = new GameObject(category + "Slider");
            sliderObject.transform.SetParent(rowObject.transform, false);
            sliderObject.AddComponent<RectTransform>();
            var slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 100000000;
            slider.wholeNumbers = true;

            var amountLabel = CreateBudgetScreenLabel(rowObject.transform, category + "AmountLabel", 16);
            var percentLabel = CreateBudgetScreenLabel(rowObject.transform, category + "PercentLabel", 16);

            SetSerializedField(_budgetPresenter, category + "Slider", slider);
            SetSerializedField(_budgetPresenter, category + "AmountLabel", amountLabel);
            SetSerializedField(_budgetPresenter, category + "PercentLabel", percentLabel);
        }

        private TMP_Text CreateBudgetScreenLabel(Transform parent, string name, int fontSize)
        {
            var labelObject = new GameObject(name);
            labelObject.transform.SetParent(parent, false);
            labelObject.AddComponent<RectTransform>();

            var textComponent = labelObject.AddComponent<TextMeshProUGUI>();
            textComponent.fontSize = fontSize;
            textComponent.color = new Color(0.1f, 0.1f, 0.2f, 1f);

            return textComponent;
        }

        private Button CreateBudgetScreenButton(Transform parent, string name, string label)
        {
            var buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent, false);
            buttonObject.AddComponent<RectTransform>();

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.15f, 0.15f, 0.3f, 1f);

            var button = buttonObject.AddComponent<Button>();

            var textObject = new GameObject("Text");
            textObject.transform.SetParent(buttonObject.transform, false);
            textObject.AddComponent<RectTransform>();

            var textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = label;
            textComponent.fontSize = 24;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = Color.white;

            return button;
        }

        private void SetSerializedField(object obj, string propertyName, object value)
        {
            var field = obj.GetType().GetField("<" + propertyName + ">k__BackingField",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
    }
}
