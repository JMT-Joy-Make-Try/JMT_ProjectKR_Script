using UnityEngine;

namespace JMT.System.GameSystem.Object
{
    public class DontDestroyOnLoadObject : MonoBehaviour
    {
        private static DontDestroyOnLoadObject _instance;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}