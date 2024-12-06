using UnityEngine;
using System.Collections;

public class CowBehavior : MonoBehaviour
{
    private bool isSelected = false;
    private bool isActive = false;
    private bool hasPopped = false;
    private QuickTimeEvent qte;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private Vector3 hiddenPosition;
    private Vector3 poppedPosition;
    private float popUpChance;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        qte = FindObjectOfType<QuickTimeEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localPosition = hiddenPosition;
        isActive = true;
    }

    public void Initialize(float yOffset, float popUpDelay, float chance)
    {
        hiddenPosition = new Vector3(0, 0.1f, 0); 
        poppedPosition = new Vector3(0, yOffset, 0); 
        popUpChance = chance;


        StartCoroutine(CheckForPopUp(popUpDelay));
    }

    IEnumerator CheckForPopUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!hasPopped && Random.value < popUpChance)
        {
            StartCoroutine(PopUpAnimation());
        }
    }

    IEnumerator PopUpAnimation()
    {
        hasPopped = true;
        float elapsed = 0;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(hiddenPosition, poppedPosition, t);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        if (!isSelected && isActive && hasPopped && qte != null)
        {
            isSelected = true;
            spriteRenderer.color = Color.yellow;
            qte.StartQuickTimeEvent(this);
        }
    }

    public void OnSuccessfulRope()
    {
        gameManager.IncrementScore();
        spriteRenderer.color = Color.green;
        StartCoroutine(FallDownAndDestroy());
    }

    public void OnFailedRope()
    {
        isSelected = false;
        spriteRenderer.color = Color.red;
        StartCoroutine(FallDownAndDestroy());
    }

    IEnumerator FallDownAndDestroy()
    {
        isActive = false;
        float elapsed = 0;
        float duration = 0.5f;
        Vector3 startPosition = transform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(startPosition, hiddenPosition, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}