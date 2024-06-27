using UnityEngine;

namespace Assets._Scripts.Core_Scripts
{
    public static class Helpers
    {
        public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
        {
            float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

            return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
        }

        public static bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) != 0;
        }
    }
}
