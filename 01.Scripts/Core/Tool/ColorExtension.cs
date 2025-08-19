using UnityEngine;

namespace JMT.Core.Tool
{
    public static class ColorExtension
    {
        public static Color ToColor(this string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                return color;
            }
            else
            {
                Debug.LogWarning($"Invalid color hex string: {hex}");
                return Color.white;
            }
        }
    }
}
