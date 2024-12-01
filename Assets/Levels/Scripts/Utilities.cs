namespace ShatterStep
{
    namespace UI
    {
        public static class Utilities
        {
            public static float GetLength(MenuInfo menuInfo)
            {
                return (menuInfo.Height + menuInfo.Spacing) * menuInfo.Children;
            }
        }

        public interface IElement
        {
            public void Setup(MenuController controller, LevelMenu select);

            public void Cleanup();
        }

        public struct MenuInfo
        {
            public int Children;
            public float Height;
            public float Spacing;
        }
    }
}