using JMT.System.DataSystem;
using TMPro;
using UnityEngine;

namespace JMT.UI.Game.Coin
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;

        private void Start()
        {
            DataManager.Instance.OnCoinCountChanged += UpdateCoinText;
            UpdateCoinText(DataManager.Instance.CoinCount);
        }

        private void OnDestroy()
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.OnCoinCountChanged -= UpdateCoinText;
            }
        }

        private void UpdateCoinText(int coinCount)
        {
            coinText.text = $"{coinCount}냥";
        }
    }
}