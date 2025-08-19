using JMT.System.EffectSystem;

namespace JMT.System.AgentSystem.PlayerSystem.Component
{
    public class PlayerParticle : ParticlePlayer<PlayerParticleType>
    {
    }

    public enum PlayerParticleType
    {
        Run,
        Hit,
        Heal
    }
}