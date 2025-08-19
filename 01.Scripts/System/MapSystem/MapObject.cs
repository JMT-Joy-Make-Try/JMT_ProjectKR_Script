using System;
using System.Collections;
using DG.Tweening;
using JMT.Core.Tool;
using JMT.System.CombatSystem;
using UnityEngine;

namespace JMT.System.MapSystem
{
    public class MapObject : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _destroyTime = 5f;
        [SerializeField] private VisibilityTracker _visibilityTracker;

        private float _defaultSpeed;
        private bool _isMoving = false;
        private MapObject _prefabReference; // 풀 반환용

        private void Start()
        {
            if (_visibilityTracker != null)
            {
                _visibilityTracker.OnVisibilityChanged += HandleVisibilityChanged;
            }
        }

        void OnDestroy()
        {
            if (_visibilityTracker != null)
            {
                _visibilityTracker.OnVisibilityChanged -= HandleVisibilityChanged;
            }
        }

        private void HandleVisibilityChanged(bool visibility)
        {
            if (!visibility)
            {
                ReturnToPool();
            }
        }

        

        public void SetPrefabReference(MapObject prefab)
        {
            _prefabReference = prefab;
        }

        private void OnEnable()
        {
            _defaultSpeed = _speed;
            _isMoving = _speed > 0;
            StartCoroutine(WaitAndDisable());
            MapManager.Instance.AddMapObject(this);
        }

        private IEnumerator WaitAndDisable()
        {
            float elapsedTime = 0f;
            while (elapsedTime < _destroyTime)
            {
                if (_isMoving)
                    elapsedTime += Time.deltaTime;
                yield return null;
            }

            ReturnToPool();
        }

        private void Update()
        {
            transform.Translate(Vector3.left * (Time.deltaTime * _speed));
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
            _isMoving = speed > 0;
        }

        public void DOSpeed(float speed, float duration)
        {
            DOVirtual.Float(_speed, speed, duration, SetSpeed);
        }

        public void SetDefaultSpeed()
        {
            _speed = _defaultSpeed;
            _isMoving = _defaultSpeed > 0;
        }

        public void SetDefaultSpeed(float duration)
        {
            DOVirtual.Float(_speed, _defaultSpeed, duration, SetSpeed);
        }

        private void ReturnToPool()
        {
            if (MapManager.Instance != null)
                MapManager.Instance.RemoveMapObject(this);

            if (_prefabReference != null && MapObjectPool.Instance != null)
                MapObjectPool.Instance.Return(_prefabReference, this);
            else
                gameObject.SetActive(false); // fallback
        }
    }
}
