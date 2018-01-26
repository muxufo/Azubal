using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombeMur : Bombe
{    
    public override void Explode()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1f);
        Destroy(gameObject);
        
        gameManager.placerRocher(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

        player.IncrementerNbrBombe();
    }

}
