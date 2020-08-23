using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersOfHanoi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        char startPeg = 'A'; // start tower in output
        char endPeg = 'C'; // end tower in output
        char tempPeg = 'B'; // temporary tower in output
        int totalDisks = 5; // number of disks

        SolveTowers(totalDisks, startPeg, endPeg, tempPeg);
    }

    void SolveTowers(int disks, char startPeg, char endPeg, char tempPeg)
    {
        if(disks > 0)
        {
            SolveTowers(disks - 1, startPeg, tempPeg, endPeg);
            Debug.Log("Move disk from " + startPeg + ' ' + endPeg);
            SolveTowers(disks - 1, tempPeg, endPeg, startPeg);
        }
    }
}
