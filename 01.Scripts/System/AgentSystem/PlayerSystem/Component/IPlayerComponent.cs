namespace JMT.System.AgentSystem.PlayerSystem.Component
{
    public interface IPlayerComponent
    {
        Player Player { get; }
        void Init(Player player);
    }
}