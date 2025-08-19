using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace JMT.System.MapSystem
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Material _groundMaterial;
        public Material GroundMaterial => _groundMaterial;

        private float _curTimer = 0f;
        private bool _isMoving = false;

        private void Start()
        {
            // _groundMaterial.DOVector(new Vector4(1f, 0, 0, 0), "_ScrollSpeed", 10f)
            //     .SetLoops(-1, LoopType.Restart);
            SetSpeed(1);
        }

        private void Update()
        {
            if (_isMoving)
            {
                _curTimer += Time.deltaTime;
            }
            else
            {
                _curTimer = 0f;
            }
            _groundMaterial.SetFloat("_Timer", _curTimer);
        }

        public void SetSpeed(float speed)
        {
            if (_groundMaterial != null)
            {
                _groundMaterial.SetVector("_ScrollSpeed", new Vector4(speed, 0, 0, 0));
            }
            else
            {
                Debug.LogWarning("Ground material is not assigned.");
            }
            _isMoving = speed != 0;
        }

        public void DOSpeed(float speed, float duration, Action onComplete = null)
        {
            if (_groundMaterial != null)
            {
                _groundMaterial.DOVector(new Vector4(speed, 0, 0, 0), "_ScrollSpeed", duration).OnComplete(() =>
                {
                    onComplete?.Invoke();
                    _isMoving = speed != 0;
                });
                //StartCoroutine(DoSpeedCoroutine(speed, duration, onComplete));
            }
            else
            {
                Debug.LogWarning("Ground material is not assigned.");
            }
        }

        private IEnumerator DoSpeedCoroutine(float speed, float duration, Action onComplete)
        {
            float elapsed = 0f;

            Vector4 startSpeed = -_groundMaterial.GetVector("_ScrollSpeed");
            Vector4 targetSpeed = new Vector4(speed, 0, 0, 0);

            Debug.Log($"Start Speed: {startSpeed.x:F2}, Target Speed: {targetSpeed.x:F2}, {duration}s");

            float a = duration;
            float c = 0f;
            float d = startSpeed.x;
            float b = targetSpeed.x;

            float scale = (b - d) / ((a - c) * (a - c));

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float x = Mathf.Min(elapsed, duration);

                //float currentX = scale * (x - c) * (x - c) + d;
                float currentX = Mathf.Lerp(startSpeed.x, targetSpeed.x, elapsed / duration);
                //currentX = Mathf.Clamp(currentX, 0f, 1f);

                //Debug.Log($"Current Speed: {currentX:F2}");

                Vector4 currentSpeed = new Vector4(currentX, 0, 0, 0);
                _groundMaterial.SetVector("_ScrollSpeed", currentSpeed);

                yield return null;
            }

            Debug.Log($"elapsed: {elapsed:F2}, duration: {duration:F2}");

            _groundMaterial.SetVector("_ScrollSpeed", targetSpeed);
            onComplete?.Invoke();
            _isMoving = speed > 0;
        }
    }
}