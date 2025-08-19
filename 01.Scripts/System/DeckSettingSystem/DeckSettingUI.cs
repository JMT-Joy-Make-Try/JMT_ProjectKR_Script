using System;
using System.Collections.Generic;
using JMT.System.Card;
using JMT.System.Card.CardData;
using JMT.System.SkillSystem;
using JMT.UISystem;
using JMT.WaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace JMT.System.DeckSettingSystem
{
    public class DeckSettingUI : FadeUI
    {
        [Header("Content Transform")]
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private RectTransform deckContent;
        [SerializeField] private RectTransform skillContent;
        [SerializeField] private RectTransform statContent;

        [Header("Deck Text")]
        [SerializeField] private TextMeshProUGUI skillCardCountText;
        [SerializeField] private TextMeshProUGUI statCardCountText;

        [Header("Card Prefabs")]
        [SerializeField] private SkillCard skillCardPrefab;
        [SerializeField] private StatCard statCardPrefab;

        [Header("Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button filterButton;

        [Header("Filter UI")]
        [SerializeField] private DeckSettingFilterUI filterUI;

        private bool isFilterOpen = false;

        private void Start()
        {
            DeckSettingManager.Instance.OnSkillCardCountChanged += UpdateSkillCardCount;
            DeckSettingManager.Instance.OnStatCardCountChanged += UpdateStatCardCount;
            closeButton.onClick.AddListener(ClosePanel);
            filterButton.onClick.AddListener(HandleFilterButtonClick);
        }

        private void HandleFilterButtonClick()
        {
            isFilterOpen = !isFilterOpen;
            if (isFilterOpen)
                filterUI.OpenPanel();
            else
                filterUI.ClosePanel();
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            InitCards(DeckSettingManager.Instance.GetSortedCards());
            InitPocketCards(DeckSettingManager.Instance.SkillCardList, DeckSettingManager.Instance.StatCardList);
            DeckSettingManager.Instance.Init();
            UpdateSkillCardCount(DeckSettingManager.Instance.CurrentSkillCardCount, DeckSettingManager.Instance.MaxSkillCards);
            UpdateStatCardCount(DeckSettingManager.Instance.CurrentStatCardCount, DeckSettingManager.Instance.MaxStatCards);
        }

        public override void ClosePanel()
        {
            var deckSettingManager = DeckSettingManager.Instance;
            if (!deckSettingManager.IsCountValid())
            {
                UIManager.Instance.MessageCompo.ShowMessage("최소 능력패 2장, 일반패 3장이 필요합니다.");
                return;
            }

            base.ClosePanel();

            foreach (Transform child in deckContent)
                Destroy(child.gameObject);

            UIManager.Instance.LogCompo.OpenPanel();
            WaveManager.Instance.WaveStart();
        }

        private void OnDestroy()
        {
            if (DeckSettingManager.Instance != null)
            {
                DeckSettingManager.Instance.OnSkillCardCountChanged -= UpdateSkillCardCount;
                DeckSettingManager.Instance.OnStatCardCountChanged -= UpdateStatCardCount;
            }
        }

        private void UpdateSkillCardCount(int currentCount, int maxCount)
        {
            skillCardCountText.text = $"능력패: {currentCount} / {maxCount}";
        }

        private void UpdateStatCardCount(int currentCount, int maxCount)
        {
            statCardCountText.text = $"일반패: {currentCount} / {maxCount}";
        }

        public void InitCards(List<CardDataSO> cards)
        {
            foreach (Transform child in deckContent)
                Destroy(child.gameObject);

            foreach (var card in cards)
            {
                card.Color = CardColor.White;
                if (card is SkillDataSO skillCardData)
                {
                    var skillCard = Instantiate(skillCardPrefab, deckContent);
                    skillCard.SetSkillCardData(skillCardData, false);
                }
                else if (card is StatCardDataSO statCardData)
                {
                    var statCard = Instantiate(statCardPrefab, deckContent);
                    statCard.SetStatCardData(statCardData, false);
                }
            }
        }

        public void InitPocketCards(SkillDataListSO skillCards, StatCardListSO statCards)
        {
            foreach (Transform child in skillContent)
                Destroy(child.gameObject);
            foreach (Transform child in statContent)
                Destroy(child.gameObject);

            foreach (var skillCard in skillCards.skillData)
            {
                skillCard.Color = CardColor.White;
                var card = Instantiate(skillCardPrefab, skillContent);
                card.SetSkillCardData(skillCard, false);
            }

            foreach (var statCard in statCards.statCardList)
            {
                statCard.Color = CardColor.White;
                var card = Instantiate(statCardPrefab, statContent);
                card.SetStatCardData(statCard, false);
            }
        }

        public void MoveCardToSkillContent(SkillCard skillCard)
        {
            AnimateCardMove(skillCard, skillContent, false); // 뒤로 미루기 없음
        }

        public void MoveCardToStatContent(StatCard statCard)
        {
            AnimateCardMove(statCard, statContent, false); // 뒤로 미루기 없음
        }

        public void MoveCardToDeckContent(BaseCard baseCard)
        {
            AnimateCardMove(baseCard, deckContent, true); // DeckContent로 옮길 때만 스킬카드 뒤로 밀기
        }

        private void AnimateCardMove(BaseCard card, RectTransform targetParent, bool moveSkillBack)
        {
            card.SetInteractable(false);
            Vector3 startPos = card.transform.position;
            Quaternion startRot = card.transform.rotation;
            Vector3 startScale = card.transform.lossyScale;

            // 카드 캔버스로 이동
            card.transform.SetParent(canvasRectTransform, true);
            card.transform.position = startPos;
            card.transform.rotation = startRot;
            card.transform.localScale = startScale;

            // sibling index 결정
            int siblingIndex = targetParent.childCount;
            if (moveSkillBack && card is SkillCard)
            {
                int lastSkillIndex = -1;
                for (int i = 0; i < targetParent.childCount; i++)
                {
                    if (targetParent.GetChild(i).GetComponent<SkillCard>() != null)
                        lastSkillIndex = i;
                }
                siblingIndex = (lastSkillIndex >= 0) ? lastSkillIndex + 1 : 0;
            }

            // 임시 dummy 생성
            GameObject dummyObj = new GameObject("CardTargetDummy");
            RectTransform dummy = dummyObj.AddComponent<RectTransform>();
            dummy.SetParent(targetParent, false);
            dummy.SetSiblingIndex(siblingIndex);

            LayoutRebuilder.ForceRebuildLayoutImmediate(targetParent);
            Vector3 targetPos = dummy.position;

            Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, (targetPos - startPos).normalized);

            float moveDuration = 0.5f;
            float rotateDuration = 0.2f;

            // 카드 이동 + 회전 + 스케일
            Sequence moveSequence = DOTween.Sequence();
            moveSequence.Append(card.transform.DOScale(startScale * 1.2f, 0.05f));
            moveSequence.Append(card.transform.DOScale(startScale, 0.05f));
            moveSequence.Join(card.transform.DOMove(targetPos, moveDuration).SetEase(Ease.InOutCubic));
            moveSequence.Join(card.transform.DORotateQuaternion(lookRot, rotateDuration).SetEase(Ease.InOutCubic));

            moveSequence.OnComplete(() =>
            {
                int finalIndex = dummy.GetSiblingIndex();
                Destroy(dummyObj);

                card.transform.SetParent(targetParent, false);
                card.transform.SetSiblingIndex(finalIndex);

                // 도착 후 스케일/회전 보정
                Sequence arrivalSeq = DOTween.Sequence();
                arrivalSeq.Append(card.transform.DORotateQuaternion(Quaternion.identity, 0.1f));
                arrivalSeq.Join(card.transform.DOScale(Vector3.one * 1.1f, 0.1f));
                arrivalSeq.Append(card.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack));

                card.transform.localPosition = Vector3.zero;
                card.SetInteractable(true);
            });
        }
    }
}
