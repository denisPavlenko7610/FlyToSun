using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class LevelUp : MonoBehaviour
    {
        private RectTransform rect;
        private Item[] items;

        private const int ITEM_COUNT = 3;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            items = GetComponentsInChildren<Item>(true);
        }
        
        public void Show()
        {
            PrepareNextLevelItems();
            SetUIVisibility(true);
            GameManager.Instance.StopGame();
            PlayLevelUpAudio();
        }
        
        public void Hide()
        {
            SetUIVisibility(false);
            GameManager.Instance.Resume();
            PlaySelectAudio();
        }

        public void Select(int index)
        {
            if (index >= 0 && index < items.Length)
            {
                items[index].OnClick();
            }
        }
        
        private void PrepareNextLevelItems()
        {
            DeactivateAllItems();

            int[] randomIndices = GetUniqueRandomIndices(ITEM_COUNT);

            for (int index = 0; index < randomIndices.Length; index++)
            {
                Item selectedItem = items[randomIndices[index]];
                ActivateItemOrSpecial(selectedItem);
            }
        }
        
        private void DeactivateAllItems()
        {
            foreach (Item item in items)
            {
                item.gameObject.SetActive(false);
            }
        }
        
        private void ActivateItemOrSpecial(Item item)
        {
            if (item.level == item.data.Damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
        
        private int[] GetUniqueRandomIndices(int count)
        {
            HashSet<int> uniqueIndices = new HashSet<int>();

            while (uniqueIndices.Count < count)
            {
                uniqueIndices.Add(Random.Range(0, items.Length));
            }

            return uniqueIndices.ToArray();
        }

        private void SetUIVisibility(bool isVisible)
        {
            rect.localScale = isVisible ? Vector3.one : Vector3.zero;
        }
        
        private void PlayLevelUpAudio()
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
            AudioManager.Instance.EffectBgm(true);
        }
        
        private void PlaySelectAudio()
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.Instance.EffectBgm(false);
        }
    }
}
