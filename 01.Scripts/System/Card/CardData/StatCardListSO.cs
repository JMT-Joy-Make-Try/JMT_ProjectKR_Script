using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.Card.CardData
{
    [CreateAssetMenu(fileName = "StatCardList", menuName = "SO/Card/StatCardList")]
    public class StatCardListSO : ScriptableObject
    {
        public List<StatCardDataSO> statCardList;

        public void Init()
        {
            for (int i = 0; i < statCardList.Count; i++)
            {
                statCardList[i] = statCardList[i].Clone();
            }
        }

        public bool Contains(StatCardDataSO statCardDataSO)
        {
            foreach (var statCard in statCardList)
            {
                if (statCard.Equals(statCardDataSO))
                {
                    return true;
                }
            }
            return false;
        }
    }
}