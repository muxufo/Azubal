using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private GameManager gameManager;

    private bool triggerable = true;
    private bool aToucheUnObjet = false;
    private int damageValue;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Invoke("DeleteParticles", 2f);
        Invoke("UnTrigger", 0.2f);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (triggerable)
        {
            if (other.GetComponent<BombeMur>())
            {
                other.GetComponent<BombeMur>().Explode(0.3f);
            }
            else if (other.GetComponent<Bombe>())
            {
                other.GetComponent<Bombe>().Explode(0.3f);
            }
            else if (other.CompareTag("Breakable"))
            {
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().ReduceLife(damageValue);
            }
        }
    }

    public void SetDamageValue(int value)
    {
        damageValue = value;
    }

    void DeleteParticles()
    {
        Destroy(gameObject);
    }

    void UnTrigger()
    {
        triggerable = false;
    }

    public bool getAToucheUnObjet()
    {
        return aToucheUnObjet;
    }
}