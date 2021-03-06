﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombeGlace : Bombe {
    
    protected override bool InstanciateExplosion(float x, float z, int damageValue) 
    {
        var aToucheObstacle = false;
        var typeObjet = gameManager.getTypeObjet(Mathf.RoundToInt(x), Mathf.RoundToInt(z));

        if (typeObjet == TYPE_OBJET.Mur || typeObjet == TYPE_OBJET.Rocher) {
            aToucheObstacle = true;
        }
        else {
            GameObject solGlace = Instantiate(gameManager.glace, gameManager.transform);
            solGlace.transform.position = new Vector3(x, 0.05f, z);

            Instantiate(explosion, new Vector3(x, transform.position.y, z), Quaternion.AngleAxis(-90, Vector3.right));
        }

        return aToucheObstacle;
    }
    
}
