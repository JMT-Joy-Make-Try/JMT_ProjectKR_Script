using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace JMT.System.CameraSystem.Module
{
    public class CameraZoomModule
    {
        private CinemachineCamera _cinemachineCamera;

        public CameraZoomModule(CinemachineCamera cinemachineCamera)
        {
            _cinemachineCamera = cinemachineCamera;
        }

        public void Zoom(float value, float duration)
        {
            DOTween.To(() => _cinemachineCamera.Lens.FieldOfView, x => _cinemachineCamera.Lens.FieldOfView = x, value, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => Debug.Log("Zoom In Complete"));
        }

        public void Zoom(float value, float duration, Vector3 position)
        {
            position.z = _cinemachineCamera.transform.position.z; // Ensure z position remains unchanged
            Sequence sequence = DOTween.Sequence();

            sequence.Join(
                DOTween.To(() => _cinemachineCamera.Lens.FieldOfView,
                           x => _cinemachineCamera.Lens.FieldOfView = x,
                           value, duration)
                       .SetEase(Ease.InOutSine)
            );

            sequence.Join(
                _cinemachineCamera.transform.DOMove(position, duration)
                       .SetEase(Ease.InOutSine)
            );
        }

    }
}