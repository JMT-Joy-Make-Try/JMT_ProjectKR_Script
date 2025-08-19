namespace JMT.System.Card.Interface
{
    public interface ICardComponent
    {
        ICard Card { get; }
        void Init(ICard card);
    }
}