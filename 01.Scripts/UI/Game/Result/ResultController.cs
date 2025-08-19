using JMT.System.CombatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JMT.UISystem.Result
{
    public class ResultController : MonoBehaviour
    {
        [SerializeField] private ResultView view;
        [SerializeField] private ResultCardView cardView;

        private void Awake()
        {
            view.OnTitleEvent += HandleTitleEvent;
        }

        private void HandleTitleEvent()
        {
            SceneManager.LoadScene("Title");
        }

        /// <summary>
        /// 플레이어가 죽고 난 후에 띄우는 결과창
        /// </summary>
        /// <param name="wave">종료된 웨이브 수(WaveManager쪽에서 가져오면 될듯함)</param>
        /// <param name="price">죽기 전까지 가지고 있는 엽전의 수</param>
        /// <param name="skillItems">죽기 전까지 가지고 있는 능력패 데이터들</param>
        /// <param name="generalItems">죽기 전까지 가지고 있는 일반패 데이터들</param>
        public void OpenPanel(int wave, int price, List<ItemSO> skillItems, List<ItemSO> generalItems)
        {
            view.OpenPanel();
            view.SetPriceText(price);
            view.SetWaveText(wave);
            cardView.SetSkillContentItem(skillItems);
            cardView.SetGeneralContentItem(generalItems);
        }

        public void ClosePanel()
        {
            SceneManager.LoadScene("Title");
        }
    }
}
