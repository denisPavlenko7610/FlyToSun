using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Goldmetal.UndeadSurvivor
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private float scanRange;  // Serialized for editor access while keeping private.
        [SerializeField] private LayerMask targetLayer;  // Serialized for editor access while keeping private.

        public Transform NearestTarget { get; private set; }  // Public property for external access.

        private void FixedUpdate()
        {
            var targets = FindTargets();
            NearestTarget = GetNearestTarget(targets);
        }

        private RaycastHit2D[] FindTargets()
        {
            return Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        }

        private Transform GetNearestTarget(RaycastHit2D[] targets)
        {
            if (targets.Length == 0) return null;  // Early return if no targets found.

            return targets
                .Select(target => target.transform)
                .OrderBy(t => Vector3.Distance(transform.position, t.position))
                .FirstOrDefault();  // Returns the closest target or null if none are found.
        }
    }
}