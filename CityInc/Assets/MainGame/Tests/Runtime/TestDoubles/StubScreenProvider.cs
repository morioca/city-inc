using UI;
using UnityEngine;

namespace TestDoubles
{
    public class StubScreenProvider : IScreenProvider
    {
        public Rect SafeArea { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public StubScreenProvider(Rect safeArea, int width, int height)
        {
            SafeArea = safeArea;
            Width = width;
            Height = height;
        }
    }
}
