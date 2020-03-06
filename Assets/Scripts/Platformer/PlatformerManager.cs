using UnityEngine;

public class PlatformerManager : MonoBehaviour
{
    public Checkpoint startingCheckpoint;

    public Checkpoint Checkpoint { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ResetCheckpoint();
    }

    public void ResetCheckpoint()
    {
        if (Checkpoint != null) Checkpoint.Uncheck();
        Checkpoint = startingCheckpoint;
    }
}
