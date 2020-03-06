using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static readonly Color checkedColor = new Color(0, 184, 255);

    private PlatformerManager gameManager;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<PlatformerManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.Checkpoint.Uncheck();
        gameManager.Checkpoint = this;
        this.Check();
    }

    public void Check()
    {
        spriteRenderer.color = checkedColor;
    }

    public void Uncheck()
    {
        spriteRenderer.color = Color.white;
    }
}
