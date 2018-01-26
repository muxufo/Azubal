using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private float initialY = 0.3f;
    private float minY = 0.2f;
    private float maxY = 0.4f;
    private bool goingUp = true;

	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 30) * Time.deltaTime);
        OscillateY();
	}

    private void OscillateY() {

        if (goingUp && transform.position.y < maxY) {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.002f, transform.position.z);
            if (transform.position.y >= maxY) {
                goingUp = false;
            }
        }
        else if (transform.position.y > minY) {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.002f, transform.position.z);
            if (transform.position.y <= minY) {
                goingUp = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            GetCollectedBy(player);
            Destroy(this.gameObject);
        }
    }

    protected virtual void GetCollectedBy(PlayerController player) {}

    public static void SpawnRandomPickup(float x, float z) {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        int nombreAleatoire = Random.Range(0, 7);
        GameObject newPickup = null;
        
        switch (nombreAleatoire) {
            case 0:
                newPickup = Instantiate(gameManager.pickupBombe, gameManager.gameObject.transform);
                break;
            case 1:
                newPickup = Instantiate(gameManager.pickupVitesse, gameManager.gameObject.transform);
                break;
            case 2:
                newPickup = Instantiate(gameManager.pickupRange, gameManager.gameObject.transform);
                break;
            case 3:
                newPickup = Instantiate(gameManager.pickupBombeMur, gameManager.gameObject.transform);
                break;
            case 4:
                newPickup = Instantiate(gameManager.pickupSuperBombe, gameManager.gameObject.transform);
                break;
            case 5:
                newPickup = Instantiate(gameManager.pickupBombeGlace, gameManager.gameObject.transform);
                break;
            case 6:
                newPickup = Instantiate(gameManager.pickupBombeGlue, gameManager.gameObject.transform);
                break;
        }

        if (newPickup != null) {
            newPickup.transform.position = new Vector3(x, 0.25f, z);
            newPickup.name = newPickup.name + " X:" + x + " Y:" + z;
        }
    }
}
