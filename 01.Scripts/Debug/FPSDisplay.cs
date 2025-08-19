using UnityEngine;

namespace JMT.DebugSystem
{
    public class FPSDisplay : MonoBehaviour
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
    float deltaTime = 0.0f;
    GUIStyle style;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float fps = 1.0f / deltaTime;
        string text = $"{fps:0.0} FPS";
        GUI.Label(new Rect(10, Screen.height - 30, 200, 30), text, style);
    }
#endif
    }
}
