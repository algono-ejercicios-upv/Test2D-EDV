using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWatermelon : MonoBehaviour
{
    public GameObject[] throwables;

    [Range(0, 1)]
    public float spawnRate = 0.01f;

    public float minSpeed = 200f;
    public float maxSpeed = 500f;

    private float RelativeSpeed => Random.Range(minSpeed, maxSpeed) * Time.deltaTime;
    private float SpawnRateToFrames => Mathf.Round(1 / spawnRate);

    private GameObject RandomThrowable => throwables[Random.Range(0, throwables.Length)];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled && Time.frameCount % SpawnRateToFrames == 0)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject newWatermelon = Instantiate(RandomThrowable, transform);
        Throw(newWatermelon);
    }

    private void Throw(GameObject newWatermelon)
    {
        newWatermelon.GetComponent<Rigidbody2D>().AddForce(transform.right * RelativeSpeed, ForceMode2D.Impulse);
    }
}
