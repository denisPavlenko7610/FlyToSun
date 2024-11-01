using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goldmetal.UndeadSurvivor
{
    public class Item : MonoBehaviour
    {
        public ItemData data;
        public int level;
        public Weapon weapon;
        public Gear gear;

        private Image icon;
        private Text textLevel;
        private Text textName;
        private Text textDesc;

        private void Awake()
        {
            AssignComponents();
            InitializeTextFields();
        }

        private void AssignComponents()
        {
            icon = GetComponentsInChildren<Image>()[1];
            icon.sprite = data.ItemIcon;

            Text[] texts = GetComponentsInChildren<Text>();
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
        }

        private void InitializeTextFields()
        {
            textName.text = data.ItemName;
        }

        private void OnEnable()
        {
            UpdateLevelText();
            UpdateDescriptionText();
        }

        private void UpdateLevelText()
        {
            textLevel.text = $"Lv.{level + 1}";
        }

        private void UpdateDescriptionText()
        {
            switch (data.Type)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    textDesc.text = string.Format(data.Description, data.Damages[level] * 100, data.Counts[level]);
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    textDesc.text = string.Format(data.Description, data.Damages[level] * 100);
                    break;
                default:
                    textDesc.text = data.Description;
                    break;
            }
        }

        public void OnClick()
        {
            switch (data.Type)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    HandleWeaponClick();
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    HandleGearClick();
                    break;
                case ItemData.ItemType.Heal:
                    HealPlayer();
                    break;
            }

            UpdateButtonInteractivity();
        }

        private void HandleWeaponClick()
        {
            if (level == 0)
            {
                CreateWeapon();
            }
            else
            {
                LevelUpWeapon();
            }
            level++;
        }

        private void CreateWeapon()
        {
            var newWeapon = new GameObject("Weapon");
            weapon = newWeapon.AddComponent<Weapon>();
            weapon.Init(data);
        }

        private void LevelUpWeapon()
        {
            float nextDamage = data.BaseDamage + data.BaseDamage * data.Damages[level];
            int nextCount = data.Counts[level];
            weapon.LevelUp(nextDamage, nextCount);
        }

        private void HandleGearClick()
        {
            if (level == 0)
            {
                CreateGear();
            }
            else
            {
                LevelUpGear();
            }
            level++;
        }

        private void CreateGear()
        {
            var newGear = new GameObject("Gear");
            gear = newGear.AddComponent<Gear>();
            gear.Init(data);
        }

        private void LevelUpGear()
        {
            float nextRate = data.Damages[level];
            gear.LevelUp(nextRate);
        }

        private void HealPlayer()
        {
            GameManager.Instance.Health = GameManager.Instance.MaxHealth;
        }

        private void UpdateButtonInteractivity()
        {
            if (level >= data.Damages.Length)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
