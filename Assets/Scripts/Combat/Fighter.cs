using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, IJsonSaveable, IModifierProvider {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;

        private void Start() {
            if (currentWeapon == null) {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) { return; }
            if (target.IsDead()) { return; }

            if (!IsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            } else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        // Public Methods 
        public bool IsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) { return false; }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return combatTarget != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        // Private Methods
        public void EquipWeapon(Weapon weapon) {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() {
            return target;
        }

        private void AttackBehaviour() {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks) {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack() {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit() {
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile()) {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            } else {
                
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot() {
            Hit();
        }

        private void StopAttack(){
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeapon.GetDamage();
            }
        }

        public JToken CaptureAsJToken() {
            return JToken.FromObject(currentWeapon.name);
        }

        public void RestoreFromJToken(JToken state) {
            string weaponName = state.ToObject<string>();
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
