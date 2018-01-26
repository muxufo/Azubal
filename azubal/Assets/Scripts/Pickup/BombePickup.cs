using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_BOMBE_PICKUP
{
    BOMBE,
    SUPER_BOMBE,
    BOMBE_MUR,
    BOMBE_GLACE,
    BOMBE_GLUANTE,
    BOMBE_DEFECTUEUSE
}

public class BombePickup : Pickup {

    public TYPE_BOMBE_PICKUP typeBombe;

    protected override void GetCollectedBy(PlayerController player) {
        if (typeBombe == TYPE_BOMBE_PICKUP.BOMBE) {
            player.IncrementerNbrBombeMax();
        }
        else {
            player.addBombeSpeciale(typeBombe);
        }
    }

}
