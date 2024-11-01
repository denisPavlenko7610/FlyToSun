using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Goldmetal.UndeadSurvivor
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Vector2 inputVec;
        [FormerlySerializedAs("speed")] [SerializeField]
        private float _speed;
        [SerializeField]
        private Scanner scanner;
        [SerializeField]
        private Hand[] hands;
        [SerializeField]
        private RuntimeAnimatorController[] animCon;

        private Rigidbody2D rigidBody;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private void Awake()
        {
            InitializeComponents();
        }

        private void OnEnable()
        {
            InitializePlayer();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsLive)
            {
                MovePlayer();
            }
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.IsLive)
            {
                UpdateAnimationAndSprite();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (GameManager.Instance.IsLive)
            {
                HandleCollisionWithDamage();
            }
        }

        public void setSpeed(float speed)
        {
            _speed = speed;
        }

        public Hand getHand(int index)
        {
            return hands[index];
        }

        public Scanner getScanner()
        {
            return scanner;
        }

        private void OnMove(InputValue value)
        {
            inputVec = value.Get<Vector2>();
        }
        
        private void InitializeComponents()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            scanner = GetComponent<Scanner>();
            hands = GetComponentsInChildren<Hand>(true);
        }
        
        private void InitializePlayer()
        {
            _speed *= Character.Speed;

            if (animCon.Length > GameManager.Instance.PlayerId)
            {
                animator.runtimeAnimatorController = animCon[GameManager.Instance.PlayerId];
            }
            else
            {
                Debug.LogError("Animator controller not found for player ID: " + GameManager.Instance.PlayerId);
            }
        }
        
        private void MovePlayer()
        {
            Vector2 nextPosition = inputVec.normalized * (_speed * Time.fixedDeltaTime);
            rigidBody.MovePosition(rigidBody.position + nextPosition);
        }
        
        private void UpdateAnimationAndSprite()
        {
            animator.SetFloat("Speed", inputVec.magnitude);

            if (inputVec.x != 0)
            {
                spriteRenderer.flipX = inputVec.x < 0;
            }
        }
        
        private void HandleCollisionWithDamage()
        {
            GameManager.Instance.Health -= Time.deltaTime * 10;

            if (GameManager.Instance.Health < 0)
            {
                HandlePlayerDeath();
            }
        }
        
        private void HandlePlayerDeath()
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}
