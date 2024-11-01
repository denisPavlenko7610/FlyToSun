using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Weapon : MonoBehaviour
    {
        public int Id { get; private set; }
        public int PrefabId { get; private set; }
        public float Damage { get; private set; }
        public int Count { get; private set; }
        public float Speed { get; set; }

        private const float InfinityPer = -100f;
        private const float RotationSpeedMultiplier = 150f;
        private const float FireRateMultiplier = 0.5f;
        private const float BulletOffset = 1.5f;

        private float _timer;
        private Player _player;

        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsLive)
                return;

            HandleWeaponRotationAndFire();
            HandleTestInput();
        }

        private void HandleWeaponRotationAndFire()
        {
            switch (Id)
            {
                case 0:
                    RotateWeapon();
                    break;
                default:
                    FireAtIntervals();
                    break;
            }
        }

        private void RotateWeapon()
        {
            transform.Rotate(Vector3.back * (Speed * Time.deltaTime));
        }

        private void FireAtIntervals()
        {
            _timer += Time.deltaTime;

            if (_timer > Speed)
            {
                _timer = 0f;
                Fire();
            }
        }

        private void HandleTestInput()
        {
            if (Input.GetButtonDown("Jump"))
            {
                LevelUp(10, 1);
            }
        }

        public void LevelUp(float damageIncrease, int countIncrease)
        {
            Damage = damageIncrease * Character.Damage;
            Count += countIncrease;

            if (Id == 0)
                Batch();

            _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        public void Init(ItemData data)
        {
            SetBasicProperties(data);
            SetWeaponProperties(data);
            SetHandSprite(data);
            _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        private void SetBasicProperties(ItemData data)
        {
            name = "Weapon " + data.ItemId;
            transform.parent = _player.transform;
            transform.localPosition = Vector3.zero;
            Id = data.ItemId;
            Damage = data.BaseDamage * Character.Damage;
            Count = data.BaseCount + Character.Count;
            PrefabId = GetPrefabId(data);
        }

        private int GetPrefabId(ItemData data)
        {
            for (int index = 0; index < GameManager.Instance.Pool.GetPoolLength(); index++)
            {
                if (data.Projectile == GameManager.Instance.Pool.Get(index))
                {
                    return index;
                }
            }
            return -1; // Or throw an exception if not found
        }

        private void SetWeaponProperties(ItemData data)
        {
            switch (Id)
            {
                case 0:
                    Speed = RotationSpeedMultiplier * Character.WeaponSpeed;
                    Batch();
                    break;
                default:
                    Speed = FireRateMultiplier * Character.WeaponRate;
                    break;
            }
        }

        private void SetHandSprite(ItemData data)
        {
            Hand hand = _player.getHand((int)data.Type);
            hand.getSpriteRendererFromHand().sprite = data.Hand;
            hand.gameObject.SetActive(true);
        }

        private void Batch()
        {
            for (int index = 0; index < Count; index++)
            {
                Transform bullet = GetBulletTransform(index);
                SetupBullet(bullet, index);
                bullet.GetComponent<Bullet>().Initialize(Damage, (int)InfinityPer, Vector3.zero);
            }
        }

        private Transform GetBulletTransform(int index)
        {
            if (index < transform.childCount)
            {
                return transform.GetChild(index);
            }
            else
            {
                Transform newBullet = GameManager.Instance.Pool.Get(PrefabId).transform;
                newBullet.parent = transform;
                return newBullet;
            }
        }

        private void SetupBullet(Transform bullet, int index)
        {
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotationVector = Vector3.forward * (360 * index) / Count;
            bullet.Rotate(rotationVector);
            bullet.Translate(bullet.up * BulletOffset, Space.World);
        }

        private void Fire()
        {
            if (!_player.getScanner().NearestTarget)
                return;

            Vector3 targetPosition = _player.getScanner().NearestTarget.position;
            Vector3 direction = (targetPosition - transform.position).normalized;

            Transform bullet = GameManager.Instance.Pool.Get(PrefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            bullet.GetComponent<Bullet>().Initialize(Damage, Count, direction);

            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
        }
    }
}
