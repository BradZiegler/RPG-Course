using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicTrigger : MonoBehaviour {
        bool hasTriggered = false;

        private void OnTriggerEnter(Collider other) {
            if (!hasTriggered && other.gameObject.tag == "Player") {
                hasTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
