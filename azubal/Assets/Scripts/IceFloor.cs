using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloor : MonoBehaviour
{
    public float ICE_FRICTION = 0.02f;

	void Start ()
    {
        Invoke("retirer", Random.Range(10f, 12f));
	}
	
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            player.SetFrictionTemp(ICE_FRICTION);
        }
    }

    private void retirer() 
    {
        Destroy(gameObject);
    }
}
