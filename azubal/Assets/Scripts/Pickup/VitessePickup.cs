using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitessePickup : Pickup {

    protected override void GetCollectedBy(PlayerController player) {
        player.AugmenterVitesse();
    }

}
