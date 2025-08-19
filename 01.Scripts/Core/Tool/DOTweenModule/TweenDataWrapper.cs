using System;
using UnityEngine;

namespace JMT.Core.Tool.DOTweenModule
{
    [Serializable]
    public class TweenDataWrapper
    {
        public TweenDataSO tweenData;
        public TweenInsertType insertType = TweenInsertType.Append;
        public float insertTime = 0f; // Insert일 때만 사용
    }
}
