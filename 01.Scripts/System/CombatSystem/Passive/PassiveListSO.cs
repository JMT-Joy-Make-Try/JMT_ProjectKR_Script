using System.Collections.Generic;
using JMT.Core.Tool;
using JMT.UISystem.Tooltip;
using UnityEngine;

namespace JMT.System.CombatSystem
{
    [CreateAssetMenu(fileName = "PassiveList", menuName = "SO/Combat/PassiveList")]
    public class PassiveListSO : ScriptableObject
    {
        public List<PassiveSO> passiveList;

        public void Init()
        {
            for (int i = 0; i < passiveList.Count; i++)
            {
                passiveList[i] = Instantiate(passiveList[i]);
            }
        }

        public float GetValue(ItemTag tag)
        {
            float totalValue = 0f;
            foreach (var passive in passiveList)
            {
                if (passive == null || passive.passive == null) continue;
                foreach (var effect in passive.passive)
                {
                    if (effect == null || effect.statEffect == null) continue;
                    if (effect.statEffect.tag == tag)
                    {
                        totalValue += effect.statEffect.GetValue();
                    }
                }
            }
            return totalValue;
        }

        public float GetChanceValue(string id)
        {
            float totalValue = 0f;
            foreach (var passive in passiveList)
            {
                if (passive == null || passive.passive == null) continue;
                foreach (var effect in passive.passive)
                {
                    if (effect == null || effect.chanceEffect == null) continue;
                    if (effect.chanceEffect.effectId == id)
                    {
                        totalValue += effect.chanceEffect.probability;
                    }
                }
            }
            return totalValue;
        }
    }
}