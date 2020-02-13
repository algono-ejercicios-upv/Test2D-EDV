using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetMessage;
    public Color highlightColor = new Color(.8f, .8f, .8f);
    private Vector3 originalScale;

    private void OnMouseEnter()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = highlightColor;
        }
    }

    private void OnMouseExit()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        originalScale = transform.localScale;
        transform.localScale = originalScale * 0.9f;
    }

    private void OnMouseUp()
    {
        transform.localScale = originalScale;
        if (targetObject != null)
        {
            targetObject.SendMessage(targetMessage);
        }
    }
}
