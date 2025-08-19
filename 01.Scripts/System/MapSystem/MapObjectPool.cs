using System.Collections.Generic;
using JMT.Core;
using UnityEngine;

namespace JMT.System.MapSystem
{
    public class MapObjectPool : MonoSingleton<MapObjectPool>
    {

        private Dictionary<MapObject, Queue<MapObject>> _poolDict = new Dictionary<MapObject, Queue<MapObject>>();

        /// <summary>
        /// 특정 프리팹을 미리 count만큼 생성해 풀에 넣어둠
        /// </summary>
        public void Preload(MapObject prefab, int count)
        {
            if (!_poolDict.ContainsKey(prefab))
                _poolDict[prefab] = new Queue<MapObject>();

            for (int i = 0; i < count; i++)
            {
                MapObject obj = Instantiate(prefab);
                obj.SetPrefabReference(prefab);
                obj.gameObject.SetActive(false);
                _poolDict[prefab].Enqueue(obj);
            }
        }

        /// <summary>
        /// 풀에서 가져오기 (없으면 생성)
        /// </summary>
        public MapObject Get(MapObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_poolDict.ContainsKey(prefab))
                _poolDict[prefab] = new Queue<MapObject>();

            MapObject obj;
            if (_poolDict[prefab].Count > 0)
            {
                obj = _poolDict[prefab].Dequeue();
                obj.transform.SetPositionAndRotation(position, rotation);
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab, position, rotation);
                obj.SetPrefabReference(prefab);
            }

            return obj;
        }

        /// <summary>
        /// 풀에 반환
        /// </summary>
        public void Return(MapObject prefab, MapObject instance)
        {
            instance.gameObject.SetActive(false);

            if (!_poolDict.ContainsKey(prefab))
                _poolDict[prefab] = new Queue<MapObject>();

            _poolDict[prefab].Enqueue(instance);
        }
    }
}
