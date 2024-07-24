using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMultiplier : MonoBehaviour
{
    public int multiplierValue;

    private void OnTriggerEnter(Collider other)
    {
        bool alreadyTriggered = false;
        //Only check multiplier box for blue team players
        if (other.gameObject.CompareTag("BlueTeam"))
        {
            //Check multiplier list on player to see if this multiplier has already been used.
            foreach (GameObject multiplier in other.GetComponent<AIController>().multipliersUsed)
            {
                if (multiplier == this.gameObject)
                {
                    alreadyTriggered = true;
                }
            }

            //If multiplier box not already triggered then add to list and spawn extra copies of self
            //New copies will be duplicates so will have same health etc.
            if (!alreadyTriggered)
            {
                other.GetComponent<AIController>().multipliersUsed.Add(this.gameObject);
                for (int i = 0; i < multiplierValue - 1; i++)
                {
                    Instantiate(other.gameObject, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
