using UnityEngine;

namespace UI
{
    /// <summary>
    /// Adjusts the attached RectTransform to fit within the device's safe area.
    /// This component handles notches, dynamic islands, and other screen cutouts automatically.
    /// </summary>
    public class SafeAreaLayout : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Rect _lastSafeArea;
        private Vector2Int _lastScreenSize;
        private IScreenProvider _screenProvider;

        /// <summary>
        /// Sets the screen provider for dependency injection (used for testing).
        /// </summary>
        /// <param name="screenProvider">The screen provider to use.</param>
        public void SetScreenProvider(IScreenProvider screenProvider)
        {
            _screenProvider = screenProvider;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _screenProvider ??= new UnityScreenProvider();
            ApplySafeArea();
        }

        private void Update()
        {
            if (IsScreenSizeChanged())
            {
                ApplySafeArea();
            }
        }

        private bool IsScreenSizeChanged()
        {
            // Stub implementation
            return false;
        }

        private void ApplySafeArea()
        {
            // Stub implementation
        }
    }
}
