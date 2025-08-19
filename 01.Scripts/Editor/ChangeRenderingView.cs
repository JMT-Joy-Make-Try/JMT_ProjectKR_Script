using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace JMT.Editor
{
    public class ChangeRenderingView
    {
        private const string path = "Assets/Settings/PC_Renderer.asset";
        private static bool _isRendering;

        [MenuItem("Tools/Rendering/Change Rendering")]
        public static void ChangeRendering()
        {
            _isRendering = true;
            ApplyRenderingChange();
        }

        [MenuItem("Tools/Rendering/Change Rendering to Pixel")]
        public static void ChangeRenderingToPixel()
        {
            _isRendering = false;
            ApplyRenderingChange();
        }

        private static void ApplyRenderingChange()
        {
            // RendererData 불러오기
            var rendererData = AssetDatabase.LoadAssetAtPath<ScriptableRendererData>(path);
            if (rendererData == null)
            {
                Debug.LogError($"RendererData not found at path: {path}");
                return;
            }

            // Feature들 순회
            foreach (var feature in rendererData.rendererFeatures)
            {
                if (feature == null) continue;

                if (feature.name == "PixelRender" || feature.name == "BlackRender")
                {
                    feature.SetActive(!_isRendering);
                    EditorUtility.SetDirty(rendererData); // 저장되도록 dirty 플래그 설정
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Rendering changed. IsRendering: {_isRendering}");
        }
    }
}
