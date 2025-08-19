using JMT.System.Card;
using JMT.System.Card.CardData;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "SO/Agent/Enemy")]
    public class EnemySO : ScriptableObject
    {
        public Enemy enemyPrefab;
        public int goldValue;
        public SkillDataSO skillCardData;
    }
}