using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goldmetal.UndeadSurvivor
{
    public class HUD : MonoBehaviour
    {
        public enum InfoType { Exp, Level, Kill, Time, Health }
        
        [SerializeField]
        private InfoType type;

        private Text myText;
        private Slider mySlider;

        private Dictionary<InfoType, Action> updateActions;

        void Awake()
        {
            myText = GetComponent<Text>();
            mySlider = GetComponent<Slider>();

            InitializeUpdateActions();
        }

        void LateUpdate()
        {
            if (updateActions.TryGetValue(type, out var updateAction))
            {
                updateAction();
            }
        }

        private void InitializeUpdateActions()
        {
            updateActions = new Dictionary<InfoType, Action>
            {
                { InfoType.Exp, UpdateExp },
                { InfoType.Level, UpdateLevel },
                { InfoType.Kill, UpdateKill },
                { InfoType.Time, UpdateTime },
                { InfoType.Health, UpdateHealth }
            };
        }

        private void UpdateExp()
        {
            if (mySlider == null) return;
            float curExp = GameManager.Instance.Experience;
            float maxExp = GameManager.Instance.NextExperience[
                Mathf.Min(GameManager.Instance.Level, GameManager.Instance.NextExperience.Length - 1)];
            mySlider.value = curExp / maxExp;
        }

        private void UpdateLevel()
        {
            if (myText == null) return;
            myText.text = $"Lv.{GameManager.Instance.Level:F0}";
        }

        private void UpdateKill()
        {
            if (myText == null) return;
            myText.text = $"{GameManager.Instance.KillCount:F0}";
        }

        private void UpdateTime()
        {
            if (myText == null) return;
            float remainTime = GameManager.Instance.MaxGameTime - GameManager.Instance.GameTime;
            int min = Mathf.FloorToInt(remainTime / 60);
            int sec = Mathf.FloorToInt(remainTime % 60);
            myText.text = $"{min:D2}:{sec:D2}";
        }

        private void UpdateHealth()
        {
            if (mySlider == null) return;
            float curHealth = GameManager.Instance.Health;
            float maxHealth = GameManager.Instance.MaxHealth;
            mySlider.value = curHealth / maxHealth;
        }
    }
}
