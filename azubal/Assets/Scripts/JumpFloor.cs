using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFloor : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private float tempsRestant;
    private bool estDesactive;

    public float dureeDesactivation;
    public Material redMaterial;
    public Material grayMaterial;

	// Use this for initialization
	void Start ()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        estDesactive = false;
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !estDesactive) {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            player.SetJumping();
            Desactiver();
        }
    }
    /*
    public bool IsDesactive()
    {
        return estDesactive;
    }
    */
    public void Desactiver()
    {
        estDesactive = true;
        meshRenderer.material = grayMaterial;
        tempsRestant = dureeDesactivation;
        Invoke("_Tick", 1f);
    }
























    private void _Tick()
    {
        tempsRestant--;

        if (tempsRestant > 0)
            Invoke("_Tick", 1f);
        else
        {
            meshRenderer.material = redMaterial;
            estDesactive = false;
        }
           
    }
}
