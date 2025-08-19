using DG.Tweening;
using JMT.Core.Tool.DOTweenModule;
using TMPro;
using UnityEngine;

namespace JMT.System.AgentSystem
{
    public class AgentDamageVFX : MonoBehaviour
    {
        [SerializeField] private TextMeshPro damageTextObject;

        public void SetColor(Color color)
        {
            if (damageTextObject != null)
            {
                damageTextObject.color = color;
                damageTextObject.enableVertexGradient = false;
            }
            else
            {
                Debug.LogWarning("Damage Text Object is not assigned.");
            }
        }

        public void SetGradient(Color topColor, Color bottomColor)
        {
            if (damageTextObject != null)
            {
                damageTextObject.enableVertexGradient = true;
                VertexGradient gradient = new VertexGradient(topColor, topColor, bottomColor, bottomColor);
                damageTextObject.colorGradient = gradient;
            }
            else
            {
                Debug.LogWarning("Damage Text Object is not assigned.");
            }
        }


        public void ShowDamage(string damage, Vector3 position)
        {
            transform.DOKill();
            if (damageTextObject == null)
            {
                Debug.LogWarning("Damage Text Object is not assigned.");
                return;
            }

            damageTextObject.text = damage;
            damageTextObject.transform.position = position;
            damageTextObject.transform.localScale = Vector3.one;

            Sequence sequence = DOTween.Sequence();

            // 시작 크기는 1로
            damageTextObject.transform.localScale = Vector3.one * 2f;

            sequence.Append(
                damageTextObject.transform.DOScale(1.3f, 0.5f)
                    .SetEase(Ease.OutBack)
            )
            .AppendInterval(0.2f)
            .Join(
                damageTextObject.transform.DOMoveY(position.y + 0.5f, 0.4f)
                    .SetEase(Ease.OutSine) // 위로 살짝 뜸
            )
            .Join(
                damageTextObject.DOFade(0f, 0.4f) // 동시에 서서히 사라짐
            )
            .AppendCallback(() => Destroy(gameObject));


        }
    }
}