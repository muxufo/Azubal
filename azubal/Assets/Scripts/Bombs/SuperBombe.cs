using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBombe : Bombe {
    
    protected override bool InstanciateExplosion(float x, float z, int damageValue)
    {
        var aToucheObstacle = false;
        var typeObjet = gameManager.getTypeObjet(Mathf.RoundToInt(x), Mathf.RoundToInt(z));

        if (typeObjet == TYPE_OBJET.Mur)
        {
            aToucheObstacle = true;
        }
        else
        {
            Instantiate(explosion, new Vector3(x, transform.position.y, z), Quaternion.AngleAxis(-90, Vector3.right)).GetComponent<Explosion>().SetDamageValue(damageValue);
        }

        return aToucheObstacle;
    }
}
