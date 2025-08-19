using Unity.Cinemachine;
using UnityEngine;

namespace JMT.System.CameraSystem.Module
{
    public class CameraInpulseModule
    {
        private CinemachineImpulseSource _impulseSource;

        public CameraInpulseModule(CinemachineImpulseSource impulseSource)
        {
            _impulseSource = impulseSource;
        }

        public void TriggerImpulse(Vector3 velocity)
        {
            if (_impulseSource != null)
            {
                _impulseSource.GenerateImpulse(velocity);
            }
            else
            {
                Debug.LogWarning("Cinemachine Impulse Source is not assigned.");
            }
        }

        public void TriggerImpulse(float force)
        {
            if (_impulseSource != null)
            {
                _impulseSource.GenerateImpulse(force);
            }
            else
            {
                Debug.LogWarning("Cinemachine Impulse Source is not assigned.");
            }
        }
    }
}
