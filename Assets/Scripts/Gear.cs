using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Gear : MonoBehaviour
    {
        [SerializeField] private ItemData.ItemType gearType; 
        [SerializeField] private float rate; 

        private const float BaseGloveSpeed = 150f; 
        private const float BaseWeaponRate = 0.5f;
        private const float BasePlayerSpeed = 3f;
        
        public void Init(ItemData data)
        {
            name = $"Gear {data.ItemId}";
            transform.SetParent(GameManager.Instance.Player.transform);
            transform.localPosition = Vector3.zero;
            
            gearType = data.Type;
            rate = data.Damages[0];
            ApplyGear();
        }
        
        public void LevelUp(float newRate)
        {
            rate = newRate;
            ApplyGear();
        }
        
        private void ApplyGear()
        {
            switch (gearType)
            {
                case ItemData.ItemType.Glove:
                    ApplyGloveEffects();
                    break;
                case ItemData.ItemType.Shoe:
                    ApplyShoeEffects();
                    break;
            }
        }
        
        private void ApplyGloveEffects()
        {
            Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

            foreach (Weapon weapon in weapons)
            {
                float newSpeed = CalculateWeaponSpeed(weapon.Id);
                weapon.Speed = newSpeed;
            }
        }
        
        private float CalculateWeaponSpeed(int weaponId)
        {
            float speed;

            switch (weaponId)
            {
                case 0:
                    speed = BaseGloveSpeed * Character.WeaponSpeed;
                    return speed + (speed * rate);
                default:
                    speed = BaseWeaponRate * Character.WeaponRate;
                    return speed * (1f - rate);
            }
        }

        private void ApplyShoeEffects()
        {
            float speed = BasePlayerSpeed * Character.Speed;
            GameManager.Instance.Player.setSpeed(speed + speed * rate);
        }
    }
}
