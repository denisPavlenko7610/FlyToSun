using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private bool isLeft; 
        [SerializeField] private SpriteRenderer spriter;

        private SpriteRenderer player;

        private static readonly Vector3 RightHandPosition = new Vector3(0.35f, -0.15f, 0);
        private static readonly Vector3 RightHandPositionReverse = new Vector3(-0.15f, -0.15f, 0);
        private static readonly Quaternion LeftHandRotation = Quaternion.Euler(0, 0, -35);
        private static readonly Quaternion LeftHandRotationReverse = Quaternion.Euler(0, 0, -135);

        void Awake()
        {
            player = GetComponentsInParent<SpriteRenderer>()[1];
        }

        void LateUpdate()
        {
            UpdateHandPositionAndRotation();
        }

        public SpriteRenderer getSpriteRendererFromHand()
        {
            return spriter;
        }
        
        private void UpdateHandPositionAndRotation()
        {
            bool isReverse = player.flipX;

            if (isLeft)
            {
                UpdateLeftHand(isReverse);
            }
            else
            {
                UpdateRightHand(isReverse);
            }
        }

        private void UpdateLeftHand(bool isReverse)
        {
            transform.localRotation = isReverse ? LeftHandRotationReverse : LeftHandRotation;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        
        private void UpdateRightHand(bool isReverse)
        {
            transform.localPosition = isReverse ? RightHandPositionReverse : RightHandPosition;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
