using JMT.System.Card.CardData;
using JMT.System.Card.Component;
using JMT.System.DeckSettingSystem.Filter;

namespace JMT.System.Card.Interface
{
    public interface ICard
    {
        CardDataSO CardData { get; }
        CardTransform CardTransform { get; }
        CardAnimator CardAnimator { get; }
        CardDragger CardDragger { get; }
        CardVisual CardVisual { get; }
        CardType CardType { get; }
    }
}