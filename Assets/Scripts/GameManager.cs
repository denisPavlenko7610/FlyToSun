using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Goldmetal.UndeadSurvivor
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("# Game Control")]
        public bool IsLive { get; private set; }
        public float GameTime { get; private set; }
        public float MaxGameTime { get; private set; } = 20f; // Changed to 20s (2 * 10)

        [Header("# Player Info")]
        public int PlayerId { get; private set; }
        public float Health { get; set; }
        public float MaxHealth { get; private set; } = 100f;
        public int Level { get; private set; }
        public int KillCount { get; set; }
        public int Experience { get; private set; }
        [FormerlySerializedAs("NextExp")] public int[] NextExperience = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

        [Header("# Game Object")]
        public PoolManager Pool;
        public Player Player;
        public LevelUp UiLevelUp;
        public Result UiResult;
        public Transform UiJoy;
        public GameObject EnemyCleaner;

        private const float GameOverDelay = 0.5f;

        private void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
        }

        public void StartGame(int playerId)
        {
            PlayerId = playerId;
            Health = MaxHealth;
            Player.gameObject.SetActive(true);
            UiLevelUp.Select(playerId % 2);
            Resume();
            PlaySound(AudioManager.Sfx.Select);
        }

        public void GameOver() => StartCoroutine(HandleGameOver());

        private IEnumerator HandleGameOver()
        {
            IsLive = false;
            yield return new WaitForSeconds(GameOverDelay);
            UiResult.gameObject.SetActive(true);
            UiResult.ShowResult(Result.ResultType.Lose);
            StopGame();
            PlaySound(AudioManager.Sfx.Lose);
        }

        public void GameVictory() => StartCoroutine(HandleGameVictory());

        private IEnumerator HandleGameVictory()
        {
            IsLive = false;
            EnemyCleaner.SetActive(true);
            yield return new WaitForSeconds(GameOverDelay);
            UiResult.gameObject.SetActive(true);
            UiResult.ShowResult(Result.ResultType.Win);
            StopGame();
            PlaySound(AudioManager.Sfx.Win);
        }

        public void RetryGame() => SceneManager.LoadScene(0);

        public void QuitGame() => Application.Quit();

        private void Update()
        {
            if (!IsLive) return;

            GameTime += Time.deltaTime;

            if (GameTime >= MaxGameTime)
            {
                GameTime = MaxGameTime;
                GameVictory();
            }
        }

        public void GainExperience()
        {
            if (!IsLive) return;

            Experience++;

            if (Experience >= NextExperience[Mathf.Min(Level, NextExperience.Length - 1)])
            {
                Level++;
                Experience = 0;
                UiLevelUp.Show();
            }
        }

        public void StopGame()
        {
            IsLive = false;
            Time.timeScale = 0;
            UiJoy.localScale = Vector3.zero;
        }

        public void Resume()
        {
            IsLive = true;
            Time.timeScale = 1;
            UiJoy.localScale = Vector3.one;
        }

        private void PlaySound(AudioManager.Sfx sound)
        {
            AudioManager.Instance.PlaySfx(sound);
            AudioManager.Instance.PlayBgm(IsLive);
        }
    }
}
