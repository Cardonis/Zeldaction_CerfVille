using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public string saveToTeleport;

    public bool canTeleport = false;

    public void SetTeleporterName(string saveName)
    {
        saveToTeleport = saveName;
        canTeleport = true;
    }

}
