using JMT.System.EffectSystem;

namespace JMT.System.AgentSystem.Enemy.Component
{
    public class EnemyParticle : ParticlePlayer<EnemyParticleType>
    {
    }

    public enum EnemyParticleType
    {
        Hit,
        Attack
    }
}