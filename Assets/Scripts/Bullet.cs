using UnityEngine;
using UnityEngine.Serialization;

namespace Goldmetal.UndeadSurvivor
{
    public class Bullet : MonoBehaviour
    {
        [FormerlySerializedAs("damage")] [SerializeField]
        private float _damage;
        [SerializeField]
        private int durability;

        public float Damage => _damage;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float damage, int durability, Vector3 direction)
        {
            this._damage = damage;
            this.durability = durability;

            if (durability >= 0)
            {
                rb.linearVelocity = direction.normalized * 15f;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy") || durability == -100)
                return;

            durability--;

            if (durability < 0)
            {
                DisableBullet();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Area") || durability == -100)
                return;

            DisableBullet();
        }

        private void DisableBullet()
        {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}