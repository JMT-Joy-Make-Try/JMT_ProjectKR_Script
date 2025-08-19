using System;
using System.Collections.Generic;
using JMT.Core;
using JMT.EventSystem;
using JMT.System.AgentSystem.PlayerSystem;
using JMT.System.MapSystem;
using JMT.WaveSystem;
using UnityEngine;

namespace JMT.System.CombatSystem
{
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private bool _isForceMapMove;
        [SerializeField] private Ground _ground;
        [SerializeField] private List<MapObject> mapObjects = new List<MapObject>();
        [SerializeField] private float mapSpeedDuration = 1f;

        public event Action<bool> OnMapMoveEvent;

        public bool IsMapMoving { get; private set; } = false;

        private void Start()
        {
            WaveManager.Instance.OnWaveTypeEvent += HandleWaveTypeEvent;
        }

        private void OnDestroy()
        {
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.OnWaveTypeEvent -= HandleWaveTypeEvent;
            }
        }

        private void HandleWaveTypeEvent(WaveType type, GameEventSO currentEventSO)
        {
            if (_isForceMapMove)
            {
                StartMove();
                return;
            }
            if (currentEventSO is FightEventSO or RepairEventSO)
            {
                StopMove();
            }
            else
            {
                StartMove();
            }
        }

        public void AddMapObject(MapObject mapObject)
        {
            if (mapObject != null && !mapObjects.Contains(mapObject))
            {
                mapObjects.Add(mapObject);
            }
            else
            {
                Debug.LogWarning("Map object is null or already exists in the list.");
            }
        }

        public void RemoveMapObject(MapObject mapObject)
        {
            if (mapObject != null && mapObjects.Contains(mapObject))
            {
                mapObjects.Remove(mapObject);
            }
            else
            {
                Debug.LogWarning("Map object is null or does not exist in the list.");
            }
        }

        public void StartMove(bool isInvoke = true)
        {
            Debug.Log("Starting map movement...");
            _ground.SetSpeed(1f);
            foreach (var mapObject in mapObjects)
            {
                if (mapObject != null)
                {
                    mapObject.SetDefaultSpeed(mapSpeedDuration);
                }
                else
                {
                    Debug.LogWarning("Map object is not assigned.");
                }
            }
            if (isInvoke)
            {
                OnMapMoveEvent?.Invoke(true); // Notify that the map has started moving
            }
            IsMapMoving = true; // Update the map moving state
        }

        public void StopMove()
        {
            Debug.Log("Stopping map movement...");
            _ground.SetSpeed(-1f);
            OnMapMoveEvent?.Invoke(false);
            _ground.DOSpeed(0f, mapSpeedDuration);

            foreach (var mapObject in mapObjects)
            {
                if (mapObject != null)
                {
                    mapObject.DOSpeed(0f, mapSpeedDuration);
                }
                else
                {
                    Debug.LogWarning("Map object is not assigned.");
                }
            }
             // Notify that the map has stopped moving
            IsMapMoving = false; // Update the map moving state
        }
    }
}
