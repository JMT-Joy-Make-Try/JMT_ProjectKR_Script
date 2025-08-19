using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JMT.Core.Tool
{
    public static class ProbabilitySelector
    {
        public static T SelectByProbability<T>(List<T> items, List<float> probabilities)
        {
            if (items == null || probabilities == null || items.Count != probabilities.Count)
            {
                throw new ArgumentException("Items and probabilities must be non-null and of the same length.");
            }

            float totalProbability = probabilities.Sum();
            if (Mathf.Approximately(totalProbability, 0f))
            {
                throw new InvalidOperationException("Total probability cannot be zero.");
            }

            float randomValue = UnityEngine.Random.Range(0f, totalProbability);
            float cumulativeProbability = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    return items[i];
                }
            }

            // Fallback in case of rounding errors
            return items.Last();
        }
    }
}