using RPG.Combat;
using UnityEngine;
using RPG.Core;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] float chaseDistance = 5f;

        GameObject player;
        Fighter fighter;
        Health health;

        private void Start() {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update() {
            if (health.IsDead()) { return; }
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
                StartAttack();
            } else {
                StopAttack();
            }
        }

        private bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void StartAttack() {
            fighter.Attack(player);
        }

        private void StopAttack() {
            fighter.Cancel();
        }
    }
}
