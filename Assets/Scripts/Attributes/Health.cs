using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, IJsonSaveable {
        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;

        LazyValue<float> healthPoints;

        bool isDead = false;

        // Called on Awake
        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        // Initilization function called by lazyvalue forceinit call
        private float GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        // Called on Start
        private void Start() {
            healthPoints.ForceInit();
        }

        // Subscribes events
        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        // Unsubscribes events
        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage) {
            print(gameObject.name + " took " + damage + " damage");
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0f);

            if (healthPoints.value == 0) {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            } else {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healthToRestore) {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }
        
        // Returns the current health points
        public float GetHealthPoints() {
            return healthPoints.value;
        }

        // Returns the max number of health points
        public float GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        // Returns current percentage of health points compared to max health points
        public float GetPercentage() {
            return 100 * GetFraction(); 
        }

        // Returns current fraction of health points compared to max health points
        public float GetFraction() {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health); 
        }

        private void Die() {
            if (isDead) { return; }

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator) {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth() {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public JToken CaptureAsJToken() {
            return JToken.FromObject(healthPoints.value);
        }

        public void RestoreFromJToken(JToken state) {
            healthPoints.value = state.ToObject<float>();
            if (healthPoints.value <= 0) {
                Die();
            }
        }
    }
}
