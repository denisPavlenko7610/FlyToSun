using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Goldmetal.UndeadSurvivor
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
    public class ItemData : ScriptableObject
    {
        public enum ItemType { Melee, Range, Glove, Shoe, Heal }

        [FormerlySerializedAs("itemType")]
        [Header("Main Info")]
        [SerializeField] private ItemType type;     
        [SerializeField] private int itemId;            
        [SerializeField] private string itemName;        
        [FormerlySerializedAs("itemDesc")] [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite itemIcon;        

        [Header("Level Data")]
        [SerializeField] private float baseDamage;       
        [SerializeField] private int baseCount;         
        [SerializeField] private float[] damages;       
        [SerializeField] private int[] counts;       

        [Header("Weapon Properties")]
        [SerializeField] private GameObject projectile; 
        [SerializeField] private Sprite hand;


        public ItemType Type => type;
        public int ItemId => itemId;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite ItemIcon => itemIcon;
        public float BaseDamage => baseDamage;
        public int BaseCount => baseCount;
        public float[] Damages => damages;
        public int[] Counts => counts;
        public GameObject Projectile => projectile;
        public Sprite Hand => hand;

        public void Validate()
        {
            if (damages.Length != counts.Length)
            {
                Debug.LogError($"Item {itemName} (ID: {itemId}) has mismatched damages and counts arrays.");
            }

        }
    }
}
