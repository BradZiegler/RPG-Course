using System.ComponentModel;
using UnityEngine;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        private void Update() {
            float healthFraction = health.GetFraction();
            if (Mathf.Approximately(healthFraction, 1) || Mathf.Approximately(healthFraction, 0)) {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthFraction, 1f, 1f);
        }
    }
}