using UnityEngine;

public class RepairsManager : MonoBehaviour
{
    public Workstation workstation;
    private GreenZone greenZone;

    public int consecutiveWins;
    public bool IsRepairing => workstation.isRotating;
    private bool UserInputDetected => Input.GetKeyDown(KeyCode.Space) 
        || Input.GetMouseButtonDown(0);

    private void Start()
    {
        workstation = GetComponentInChildren<Workstation>();
        greenZone = GetComponentInChildren<GreenZone>();
    }

    public void StopStart()
    {
        if (workstation.isRotating)
        {
            workstation.StopRotating();
        }
        else
        {
            workstation.StartRotating();
        }
    }

    public void PlayerWins()
    {
        consecutiveWins++;
        workstation.IncreaseRotationSpeed();

        // Decrease green zone size every n wins 
        if (IsGreenZoneShrinking())
        {
            greenZone.ReduceSize();
        }

        if (IsDirectionReversing())
        {
            workstation.ReverseRotationDirection();
        }

        Debug.Log("You win!");
    }

    public void PlayerLoses()
    {
        consecutiveWins = 0;
        workstation.ResetRotationSpeed();
        greenZone.ResetSize();

        Debug.Log("You lose!");
    }

    public bool IsGreenZoneShrinking()
    {
        return consecutiveWins % RepairsConstants.GreenZoneShrinkInterval == 0
            && consecutiveWins > 0;
    }

    public bool IsDirectionReversing()
    {
        return Random.Range(0, RepairsConstants.RotationReversalUpperBound)
            > RepairsConstants.RotationReversalThreshold;
    }

    private void Update()
    {
        if (UserInputDetected)
        {
            StopStart();
        }
    }
}
