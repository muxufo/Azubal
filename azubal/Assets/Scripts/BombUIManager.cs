using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUIManager : MonoBehaviour {

    public Sprite superBombe;
    public Sprite wallBomb;
    public Sprite stickyBomb;
    public Sprite iceBomb;
    public Sprite emptyImage;

    public Sprite GetSpriteBomb(TYPE_BOMBE_PICKUP bombType)
    {
        switch(bombType)
        {
            case TYPE_BOMBE_PICKUP.BOMBE_MUR :
                return wallBomb;
            case TYPE_BOMBE_PICKUP.SUPER_BOMBE:
                return superBombe;
            case TYPE_BOMBE_PICKUP.BOMBE_GLUANTE:
                return stickyBomb;
            case TYPE_BOMBE_PICKUP.BOMBE_GLACE:
                return iceBomb;
            default:
                return emptyImage;
        }
    }
}
