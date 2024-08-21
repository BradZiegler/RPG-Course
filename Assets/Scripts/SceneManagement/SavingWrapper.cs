using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour {
        [SerializeField] float fadeInTime = 0.5f;

        const string defaultSaveFile = "save";

        private IEnumerator Start() {
            Fader fader = FindFirstObjectByType<Fader>();

            fader.FadeOutImmediate();
            yield return GetComponent<JsonSavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
        }

        public void Save() {
            GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
        }

        public void Load() {
            GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
        }
    }
}