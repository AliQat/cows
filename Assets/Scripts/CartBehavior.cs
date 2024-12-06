using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBehavior : MonoBehaviour
{
    public float trackSpeed = 3f;
    public float spawnInterval = 4f;
    private float endX;

    public void Initialize(float endXPosition)
    {
        endX = endXPosition;
    }
    void Start()
    {
        GameObject mask = new GameObject("Mask");
        mask.transform.parent = transform;
        mask.transform.localPosition = Vector3.zero;

        SpriteRenderer maskRenderer = mask.AddComponent<SpriteRenderer>();
        maskRenderer.sprite = Resources.Load<Sprite>("Sprites/cart");
        maskRenderer.sortingLayerName = "Cart";
        maskRenderer.sortingOrder = 1; 
    }

    void Update()
    {
        transform.Translate(Vector2.right * trackSpeed * Time.deltaTime);

        if (transform.position.x > endX)
        {
            Destroy(gameObject);
        }
    }
}
