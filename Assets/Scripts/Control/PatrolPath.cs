using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {
        const float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos() {
            Gizmos.color = Color.white;

            for (int i = 0; i < transform.childCount; i++) {
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
            }
        }
    }
}