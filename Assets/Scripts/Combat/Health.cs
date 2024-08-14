using UnityEngine;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            if (healthPoints == 0 && !isDead) {
                Die();
            }
        }

        void Die() {
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }
    }
}
