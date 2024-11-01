using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Follow : MonoBehaviour
    {
        RectTransform rect;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        void FixedUpdate()
        {
            rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        }
    }
}
