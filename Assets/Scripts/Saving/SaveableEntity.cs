using UnityEditor;
using UnityEngine;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour{
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier() {
            return uniqueIdentifier;
        }

        public object CaptureState() {
            return null;
        }

        public void RestoreState(object state) {

        }

#if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject)) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue)) {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}