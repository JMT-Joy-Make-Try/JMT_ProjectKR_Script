using System;
using System.Collections;
using System.Collections.Generic;
using JMT.Core.Tool;
using JMT.System.CombatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JMT.System.MapSystem
{
    public class MapObjectSpawner : MonoBehaviour
    {
        [SerializeField] private MapSpawnData initializeMapSpawnData;
        public List<MapSpawnData> mapSpawnDataList = new List<MapSpawnData>();

        private bool _isSpawnEnabled = true;

        private void Start()
        {
            // 프리로드
            PreloadAll();

            // 초기 스폰
            MapObjectPool.Instance.Get(
                initializeMapSpawnData.mapObjectPrefabs[0],
                initializeMapSpawnData.spawnPoint.position,
                Quaternion.identity
            );

            MapManager.Instance.OnMapMoveEvent += HandleMapMoveEvent;

            // 각 스폰 데이터별 코루틴 시작
            foreach (var spawnData in mapSpawnDataList)
            {
                StartCoroutine(SpawnCoroutine(spawnData));
            }
        }

        private void OnDestroy()
        {
            if (MapManager.Instance != null)
                MapManager.Instance.OnMapMoveEvent -= HandleMapMoveEvent;
        }

        private void HandleMapMoveEvent(bool move)
        {
            _isSpawnEnabled = move;
        }

        /// <summary>
        /// 모든 스폰 데이터에 대해 프리로드
        /// </summary>
        private void PreloadAll()
        {
            // 나머지 데이터
            foreach (var spawnData in mapSpawnDataList)
            {
                foreach (var prefab in spawnData.mapObjectPrefabs)
                {
                    MapObjectPool.Instance.Preload(prefab, 10);
                }
            }
        }

        private IEnumerator SpawnCoroutine(MapSpawnData spawnData)
        {
            while (true)
            {
                if (!_isSpawnEnabled)
                {
                    yield return null;
                    continue;
                }

                if (spawnData.mapObjectPrefabs.Count > 0)
                {
                    int randomIndex = Random.Range(0, spawnData.mapObjectPrefabs.Count);
                    var prefab = spawnData.mapObjectPrefabs[randomIndex];
                    MapObjectPool.Instance.Get(prefab, spawnData.spawnPoint.position, Quaternion.identity);
                }

                float waitTime = spawnData.isRandomInterval
                    ? Random.Range(spawnData.spawnIntervalMin, spawnData.spawnIntervalMax)
                    : spawnData.spawnInterval;

                yield return WaitForSecondsCache.Get(waitTime);
            }
        }
    }

    [Serializable]
    public struct MapSpawnData
    {
        public Transform spawnPoint;
        public List<MapObject> mapObjectPrefabs;

        public bool isRandomInterval;
        public float spawnInterval;
        public float spawnIntervalMin;
        public float spawnIntervalMax;
    }
}
