using JMT.System.CombatSystem;
using UnityEngine;

namespace JMT.System.Card.CardData
{
    public class CardDataSO : ItemSO
    {
        public CardColor Color;

        public virtual void Init()
        {
            Color = (CardColor)Random.Range(0, 2);
        }

        public override bool Equals(object other)
        {
            if (other is CardDataSO otherCard)
            {
                return this.Name == otherCard.Name;
            }
            return false;
        }
    }

    public enum CardColor
    {
        Red,
        White
    }
}