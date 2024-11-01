using System.Collections;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Properties")]
        public float speed;
        public float health;
        public float maxHealth;
        public RuntimeAnimatorController[] animatorControllers;

        private Rigidbody2D target;
        private Rigidbody2D rigidBody;
        private Collider2D collider;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private WaitForFixedUpdate wait;

        private bool isAlive;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            wait = new WaitForFixedUpdate();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.IsLive || !isAlive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                return;

            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            Vector2 direction = target.position - rigidBody.position;
            Vector2 nextPosition = direction.normalized * (speed * Time.fixedDeltaTime);
            rigidBody.MovePosition(rigidBody.position + nextPosition);
            rigidBody.linearVelocity = Vector2.zero;
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.IsLive && isAlive)
                spriteRenderer.flipX = target.position.x < rigidBody.position.x;
        }

        private void OnEnable()
        {
            target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
            ResetEnemyState();
        }

        private void ResetEnemyState()
        {
            isAlive = true;
            collider.enabled = true;
            rigidBody.simulated = true;
            spriteRenderer.sortingOrder = 2;
            animator.SetBool("Dead", false);
            health = maxHealth;
        }

        public void Init(SpawnData data)
        {
            animator.runtimeAnimatorController = animatorControllers[data.spriteType];
            speed = data.speed;
            maxHealth = data.health;
            health = data.health;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet") && isAlive)
            {
                TakeDamage(collision.GetComponent<Bullet>().Damage);
            }
        }

        private void TakeDamage(float damage)
        {
            health -= damage;
            StartCoroutine(KnockBack());

            if (health > 0)
            {
                animator.SetTrigger("Hit");
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            else
            {
                Die();
            }
        }

        private void Die()
        {
            isAlive = false;
            collider.enabled = false;
            rigidBody.simulated = false;
            spriteRenderer.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.Instance.KillCount++;
            GameManager.Instance.GainExperience();

            if (GameManager.Instance.IsLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }

        private IEnumerator KnockBack()
        {
            yield return wait;
            Vector3 playerPosition = GameManager.Instance.Player.transform.position;
            Vector3 knockBackDirection = transform.position - playerPosition;
            rigidBody.AddForce(knockBackDirection.normalized * 3f, ForceMode2D.Impulse);
        }

        private void Dead()
        {
            gameObject.SetActive(false);
        }
    }
}
