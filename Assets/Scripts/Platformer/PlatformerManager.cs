using UnityEngine;

public class PlatformerManager : MonoBehaviour
{
    public Checkpoint startingCheckpoint;

    public Checkpoint Checkpoint { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Checkpoint = startingCheckpoint;   
    }
}
