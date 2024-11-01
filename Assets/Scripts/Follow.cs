using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Follow : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Camera mainCamera;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            mainCamera = Camera.main; // Cache the main camera reference
        }

        private void LateUpdate()
        {
            if (mainCamera && GameManager.Instance && GameManager.Instance.Player)
            {
                Vector3 playerScreenPosition = mainCamera.WorldToScreenPoint(GameManager.Instance.Player.transform.position);
                rectTransform.position = playerScreenPosition;
            }
        }
    }
}