using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public int teleporterNumber = 0;
    public int teleportPart = 0;
    public bool canTeleport = false;

    public void SetTeleporterNumber(int telNumb)
    {
        teleporterNumber = telNumb;
        canTeleport = true;
    }

    public void SetPartNumber(int telPart)
    {
        teleportPart = telPart;
    }

}
