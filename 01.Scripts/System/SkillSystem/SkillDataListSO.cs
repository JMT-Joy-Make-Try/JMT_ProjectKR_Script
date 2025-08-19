using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.SkillSystem
{
    [CreateAssetMenu(fileName = "SkillDataSO", menuName = "SO/Skill/SkillDataListSO", order = 1)]
    public class SkillDataListSO : ScriptableObject
    {
        public List<SkillDataSO> skillData;

        public void Init()
        {
            for (int i = 0; i < skillData.Count; i++)
            {
                skillData[i] = Instantiate(skillData[i]);
            }
        }

        public bool Contains(SkillDataSO skillDataSO)
        {
            foreach (var skill in skillData)
            {
                if (skill.Equals(skillDataSO))
                {
                    return true;
                }
            }
            return false;
        }
    }
}