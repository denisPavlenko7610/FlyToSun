using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class AchieveManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] lockedCharacters;
        [SerializeField]
        private GameObject[] unlockedCharacters;
        [SerializeField]
        private GameObject uiNotice;

        private enum Achieve { UnlockPotato, UnlockBean }
        private Dictionary<Achieve, Action> achieveChecks;
        private WaitForSecondsRealtime wait;

        private void Awake()
        {
            wait = new WaitForSecondsRealtime(5);

            if (!PlayerPrefs.HasKey("MyData"))
            {
                Init();
            }

            InitializeAchieveChecks();
        }

        private void Init()
        {
            PlayerPrefs.SetInt("MyData", 1);

            foreach (Achieve achieve in Enum.GetValues(typeof(Achieve)))
            {
                PlayerPrefs.SetInt(achieve.ToString(), 0);
            }
        }

        private void Start()
        {
            UnlockCharacters();
        }

        private void UnlockCharacters()
        {
            for (int i = 0; i < lockedCharacters.Length; i++)
            {
                var achieve = (Achieve)i;
                bool isUnlocked = PlayerPrefs.GetInt(achieve.ToString()) == 1;
                lockedCharacters[i].SetActive(!isUnlocked);
                unlockedCharacters[i].SetActive(isUnlocked);
            }
        }

        private void LateUpdate()
        {
            foreach (var achieve in achieveChecks.Keys)
            {
                achieveChecks[achieve].Invoke();
            }
        }

        private void InitializeAchieveChecks()
        {
            achieveChecks = new Dictionary<Achieve, Action>
            {
                { Achieve.UnlockPotato, CheckUnlockPotato },
                { Achieve.UnlockBean, CheckUnlockBean }
            };
        }

        private void CheckUnlockPotato()
        {
            if (GameManager.Instance.IsLive && GameManager.Instance.KillCount >= 20)
            {
                UnlockAchieve(Achieve.UnlockPotato);
            }
        }

        private void CheckUnlockBean()
        {
            if (Mathf.Approximately(GameManager.Instance.GameTime, GameManager.Instance.MaxGameTime))
            {
                UnlockAchieve(Achieve.UnlockBean);
            }
        }

        private async void UnlockAchieve(Achieve achieve)
        {
            if (PlayerPrefs.GetInt(achieve.ToString()) == 0)
            {
                PlayerPrefs.SetInt(achieve.ToString(), 1);
                ShowUnlockNotice((int)achieve);
                await NoticeRoutine();
            }
        }

        private void ShowUnlockNotice(int index)
        {
            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                uiNotice.transform.GetChild(i).gameObject.SetActive(i == index);
            }
        }

        private async Task NoticeRoutine()
        {
            uiNotice.SetActive(true);
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
            
            await Task.Delay((int)(wait.waitTime * 1000)); 

            uiNotice.SetActive(false);
        }
    }
}