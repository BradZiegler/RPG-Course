using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            if (healthPoints == 0 && !isDead) {
                Die();
            }
        }

        void Die() {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
