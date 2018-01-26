using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTriggerOnLeave : MonoBehaviour {
   
    void OnTriggerExit ()
    {
        GetComponent<Collider>().isTrigger = false;	
	}
}
