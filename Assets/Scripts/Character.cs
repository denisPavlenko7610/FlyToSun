using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Character : MonoBehaviour
    {
        private static GameManager gameManager => GameManager.Instance;

        public static float Speed => GetPlayerStat(1.1f, 1f);
        public static float WeaponSpeed => GetPlayerStat(1.1f, 1f, 1);
        public static float WeaponRate => GetPlayerStat(0.9f, 1f, 1);
        public static float Damage => GetPlayerStat(1.2f, 1f, 2);
        public static int Count => GetPlayerStat(1, 0, 3);

        private static T GetPlayerStat<T>(T playerValue, T defaultValue, int? playerId = null)
        {
            return gameManager.PlayerId == playerId ? playerValue : defaultValue;
        }
    }
}