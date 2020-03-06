using System.Collections;
using UnityEngine;

public class RocketAnimation : MonoBehaviour
{
    private const float _playerSpotX = 1.51f, _playerSpotY = 1.49f;
    private static readonly Vector2 playerSpot = new Vector2(_playerSpotX, _playerSpotY);

    private PlatformerManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<PlatformerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(StartAnimation(collision));
        }
    }

    private IEnumerator StartAnimation(Collider2D collision)
    {
        collision.transform.position = transform.TransformPoint(playerSpot);
        collision.transform.rotation = Quaternion.identity;

        PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
        playerMovement.Freeze();

        Debug.Log("YOU WON!");

        yield return new WaitForSeconds(2);

        gameManager.ResetCheckpoint();
        playerMovement.Respawn();
        playerMovement.Unfreeze();
    }
}
