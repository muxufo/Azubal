using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFloor : MonoBehaviour {

    void Start() {
        Invoke("retirer", 5.0f);
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            player.ReduceMovementSpeed();
        }
    }
    
    private void retirer() {
        Destroy(gameObject);
    }
}
