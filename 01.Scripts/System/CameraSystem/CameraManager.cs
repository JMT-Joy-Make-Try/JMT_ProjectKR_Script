using JMT.Core;
using JMT.System.CameraSystem.Module;
using Unity.Cinemachine;
using UnityEngine;

namespace JMT.System.CameraSystem
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        private CameraInpulseModule _cameraImpulseModule;
        private CameraZoomModule _cameraZoomModule;

        public CameraInpulseModule ImpulseModule => _cameraImpulseModule;
        public CameraZoomModule ZoomModule => _cameraZoomModule;
        public CinemachineCamera CinemachineCamera => _cinemachineCamera;

        protected override void Awake()
        {
            base.Awake();
            _cameraImpulseModule = new CameraInpulseModule(_cinemachineImpulseSource);
            _cameraZoomModule = new CameraZoomModule(_cinemachineCamera);
        }
    }
}