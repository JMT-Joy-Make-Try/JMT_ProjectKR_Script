using DG.Tweening;
using JMT.System.AgentSystem.Interface;
using JMT.System.CombatSystem;
using JMT.UISystem;
using UnityEngine;

namespace JMT.System.MapSystem
{
    public class RepairMapObject : MonoBehaviour, ISpawnable
    {
        [SerializeField] private Vector3 _targetPos;
        [SerializeField] private Ease _easeType = Ease.OutQuad;
        [SerializeField] private float _moveDuration;

        private void Start()
        {
            OnSpawn();
            UIManager.Instance.LogCompo.OnRepairEndEvent += RepairEnd;
        }

        void OnDestroy()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.LogCompo.OnRepairEndEvent -= RepairEnd;
            }
        }

        public void OnSpawn()
        {
            transform.DOMove(_targetPos, _moveDuration).SetEase(_easeType);
        }

        public void RepairEnd()
        {
            MapManager.Instance.StartMove();
            transform.DOMove(_targetPos - new Vector3(30, 0), _moveDuration).OnComplete(() =>
            {
                Destroy(gameObject);
            }).SetEase(_easeType);
        }
    }
}