using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    public int Id { get; private set; }

    [SerializeField]
    private GameObject cardBack;

    private SceneController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<SceneController>();
    }

    private void OnMouseDown()
    {
        if (cardBack.activeSelf && controller.CanReveal)
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }

    public void SetCard(int id, Sprite image)
    {
        Id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }
}
