using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour
{
    [field: SerializeField]
    public bool IsObstacle { get; private set; }

    public bool WalkedOn { get; private set; }

    public void SetStatus(bool status)
    {
        WalkedOn = status;
    }
}
