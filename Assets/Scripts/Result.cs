using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Result : MonoBehaviour
    {
        [SerializeField] private GameObject[] titles;  // Serialize to configure in the editor.

        public enum ResultType
        {
            Lose,
            Win
        }

        public void ShowResult(ResultType resultType)
        {
            // Deactivate all titles first
            foreach (GameObject title in titles)
            {
                title.SetActive(false);
            }

            // Activate the appropriate title
            int index = (int)resultType;  // Enum index corresponds to titles array index.
            if (index >= 0 && index < titles.Length && titles[index] != null)
            {
                titles[index].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Invalid result type or title is not assigned.");
            }
        }
    }
}