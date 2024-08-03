using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    [SerializeField] Transform target;

    void LateUpdate() {
        transform.position = target.position;
    }
}
