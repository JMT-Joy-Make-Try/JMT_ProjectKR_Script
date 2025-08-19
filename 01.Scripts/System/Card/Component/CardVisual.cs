using JMT.Core;
using JMT.System.Card.CardData;
using JMT.System.Card.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.System.Card.Component
{
    public class CardVisual : MonoBehaviour, ICardComponent
    {
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _cardVisual;
        [SerializeField] private Image _cardIcon;
        [SerializeField] private Material _cardVisualMat;
        [SerializeField, ColorUsage(true, true)] private Color _redColor = Color.red;
        [SerializeField, ColorUsage(true, true)] private Color _blackColor = Color.black;
        public ICard Card { get; private set; }

        private bool _isFlipped;


        public void Init(ICard card)
        {
            Card = card;
            _isFlipped = false;
            _cardVisualMat = Instantiate(_cardVisual.material);
            _cardVisual.material = _cardVisualMat;
        }

        public void SetVisual(CardDataSO cardData = null)
        {
            var card = cardData != null ? cardData : Card.CardData;

            Color color = card.Color == CardColor.Red ? _redColor : _blackColor;

            _cardVisualMat.SetColor(ShaderConst.Color, color);
            _cardIcon.color = color;

            if (_cardIcon == null)
            {
                Debug.LogWarning("CardIcon is not assigned in CardVisual.");
                return;
            }
            _cardIcon.sprite = card.Icon;
        }

        public void FlipCard(bool isFlipped)
        {
            _isFlipped = isFlipped;
        }

        public void SetActive(bool isActive)
        {
            _cardVisual.gameObject.SetActive(isActive);
            _cardIcon.gameObject.SetActive(isActive);
            _cardImage.gameObject.SetActive(isActive);
        }
    }
}