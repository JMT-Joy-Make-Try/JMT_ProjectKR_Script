using System.Collections.Generic;
using JMT.System.Card.CardData;
using JMT.System.DataSystem;
using UnityEngine;

namespace JMT.System.Card.CardSpawn
{
    public class DeckSpawnModule
    {
        private DataManager dataManager;
        public DeckSpawnModule(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public Stack<StatCardDataSO> SpawnDeck(int cardCount)
        {
            var statCards = new Stack<StatCardDataSO>();
            for (int i = 0; i < cardCount; i++)
            {
                //var statCard = Instantiate(_statCardPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);
                var statSO = dataManager.GetRandomStatCard();
                //statCard.CardDragger.SetDraggable(false);
                //statCard.SetCardData(statSO);
                statCards.Push(statSO);
            }
            return statCards;
        }
    }
}