using System.Collections;
using System.Collections.Generic;
using JMT.Core.Tool;
using JMT.System.Card;
using JMT.System.CombatSystem;
using JMT.System.SkillSystem;
using JMT.UISystem;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy.Boss
{
    [CreateAssetMenu(fileName = "CardChangeSkill", menuName = "SO/Agent/Boss/Skills/CardChangeSkill")]
    public class CardChangeSkill : SkillSO
    {
        public override void ExecuteSkill()
        {
            // UI 열기
            UIManager.Instance.DeckCompo.OpenPanel();

            // UIManager를 통해 코루틴 실행 (안전)
            UIManager.Instance.StartCoroutine(ChangeCardCoroutine());
        }

        private IEnumerator ChangeCardCoroutine()
        {
            // 3초 대기
            yield return WaitForSecondsCache.Get(3f);

            // 무효 판정
            float evasionChance = CombatManager.Instance.Player.PlayerPassive.PassiveList
                .GetValue(UISystem.Tooltip.ItemTag.InterferenceEvasion);

            float roll = UnityEngine.Random.Range(0f, 100f);
            bool isNullified = roll < evasionChance;

            if (isNullified)
            {
                Debug.LogWarning(
                    $"[카드 체인지] 스킬 무효화됨! (회피 확률 {evasionChance:F1}%, 뽑은 값 {roll:F1})");
                yield break;
            }

            // 슬롯 선택
            var cardSlots = CardSlotManager.Instance.CardSlots;
            if (cardSlots == null || cardSlots.Count < 2)
            {
                Debug.LogWarning("[카드 체인지] 슬롯이 부족하여 실행 불가.");
                yield break;
            }

            int targetSlotIndex = UnityEngine.Random.Range(0, cardSlots.Count);

            // 다른 슬롯 목록 (타겟 제외)
            List<int> availableSlotIndices = new List<int>();
            for (int i = 0; i < cardSlots.Count; i++)
            {
                if (i != targetSlotIndex)
                    availableSlotIndices.Add(i);
            }

            int anotherSlotIndex = availableSlotIndices[
                UnityEngine.Random.Range(0, availableSlotIndices.Count)
            ];

            Debug.Log($"[카드 체인지] 발동! (대상 슬롯: {targetSlotIndex}, 교체 슬롯: {anotherSlotIndex})");

            // 대체 카드 목록 가져오기
            var skillCardDataList = CardSlotManager.Instance.GetAnotherSkillCards(targetSlotIndex);
            if (skillCardDataList.Count == 0)
            {
                Debug.LogWarning("[카드 체인지] 대체 가능한 카드 없음.");
                yield break;
            }

            // 대체 카드 선택
            var skillCardData = skillCardDataList[UnityEngine.Random.Range(0, skillCardDataList.Count)];
            if (skillCardData == null)
            {
                Debug.LogWarning("[카드 체인지] 선택된 대체 카드가 null.");
                yield break;
            }

            // 슬롯 교체 실행
            var targetSlotData = cardSlots[targetSlotIndex].GetSkillCardData();
            CardSlotManager.Instance.SetSkillCard(anotherSlotIndex, targetSlotData);
            CardSlotManager.Instance.SetSkillCard(targetSlotIndex, skillCardData);

            Debug.Log($"[카드 체인지] 카드 교체 완료! (슬롯 {targetSlotIndex} ← {skillCardData.Name}, 슬롯 {anotherSlotIndex} ← {targetSlotData.Name})");
        }

        public override void Init(SkillType skillType, float damage)
        {
            // Not used
        }
    }
}
