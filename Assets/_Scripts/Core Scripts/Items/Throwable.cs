using Assets._Scripts.Core_Scripts;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Items
{
    public class Throwable : MonoBehaviour
    {
        public int Damage;
        public LayerMask TargetLayer;
        public float LifeTime = 5f;
        private float _timer;

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= LifeTime)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!Helpers.IsInLayerMask(other.gameObject, TargetLayer)) return;

            var entity = other.gameObject.GetComponentInParent<EntityData>();
            if (entity != null)
                entity.TakeDamage(Damage);

            Destroy(gameObject);
        }
    }
}
