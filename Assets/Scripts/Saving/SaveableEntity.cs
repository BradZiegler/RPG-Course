using UnityEngine;

namespace RPG.Saving {
    public class SaveableEntity : MonoBehaviour{
        public string GetUniqueIdentifier() {
            return "";
        }

        public object CaptureState() {

            return null;
        }

        public void RestoreState(object state) {
            
        }
    }
}