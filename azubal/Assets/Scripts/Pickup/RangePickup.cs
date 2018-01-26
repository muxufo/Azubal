using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangePickup : Pickup {

    protected override void GetCollectedBy(PlayerController player) {
        player.AugmenterRange();
    }

}
