using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    int timesPlayerHasToMoveToActivate = 2;

    public void PlayerMoved()
    {
        timesPlayerHasToMoveToActivate--;

        if (timesPlayerHasToMoveToActivate == 0)
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    public bool TrapActivated()
    {
        if (timesPlayerHasToMoveToActivate == 0)
            return true;
        else
            return false;
    }
}
