using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombe : MonoBehaviour {
    public float id;
    public int range;
    public const float EXPLOSION_DELAY = 4f;
    public GameObject explosion;
    public AudioClip explosionSound;
    public PlayerController player;

    protected const int EXPLOSION_DAMAGE = 12;

    protected GameManager gameManager;
    
    //public GameObject sphere;

    // Use this for initialization
    void Start()
    {
        Invoke("Explode", EXPLOSION_DELAY);
        GetComponent<AudioSource>().Play();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public virtual void Explode()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1f);
        Destroy(gameObject);

        InstanciateExplosion(transform.position.x, transform.position.z, EXPLOSION_DAMAGE);
        for (var direction = 0; direction < 4; direction++)
        {
            for (var distance = 1; distance < range+1; distance++)
            {
                bool estMurAtteint = false;
                switch(direction)
                {
                    case 0:
                        estMurAtteint = InstanciateExplosion(transform.position.x + distance, transform.position.z, EXPLOSION_DAMAGE - distance);
                        break;
                    case 1:
                        estMurAtteint = InstanciateExplosion(transform.position.x - distance, transform.position.z, EXPLOSION_DAMAGE - distance);
                        break;
                    case 2:
                        estMurAtteint = InstanciateExplosion(transform.position.x, transform.position.z + distance, EXPLOSION_DAMAGE - distance);
                        break;
                    case 3:
                        estMurAtteint = InstanciateExplosion(transform.position.x, transform.position.z - distance, EXPLOSION_DAMAGE - distance);
                        break;
                }
                if (estMurAtteint)
                {
                    break;
                }
            }
        }
        player.IncrementerNbrBombe();
    }

    public void Explode(float timer)
    {
        Invoke("Explode", timer);
    }

    protected virtual bool InstanciateExplosion(float x, float z, int damageValue)
    {
        var aToucheObstacle = false;
        var typeObjet = gameManager.getTypeObjet(Mathf.RoundToInt(x), Mathf.RoundToInt(z));

        if (typeObjet == TYPE_OBJET.Mur)
        {
            aToucheObstacle = true;
        }
        else if (typeObjet == TYPE_OBJET.Rocher)
        {
            aToucheObstacle = true;
            Instantiate(explosion, new Vector3(x, transform.position.y, z), Quaternion.AngleAxis(-90, Vector3.right)).GetComponent<Explosion>().SetDamageValue(damageValue);
            gameManager.RetirerObjet(Mathf.RoundToInt(x), Mathf.RoundToInt(z));
        }
        else
        {
            Instantiate(explosion, new Vector3(x, transform.position.y, z), Quaternion.AngleAxis(-90, Vector3.right)).GetComponent<Explosion>().SetDamageValue(damageValue);
        }

        return aToucheObstacle;
    }
}
