using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuickTimeEvent : MonoBehaviour
{
    public Text promptText;
    public float timePerKey = 1.5f;

    private string[] keySequence = new string[6];
    private int currentKeyIndex = 0;
    private bool isActive = false;
    private CowBehavior currentCow;

    public void StartQuickTimeEvent(CowBehavior cow)
    {
        if (!isActive)
        {
            currentCow = cow;
            isActive = true;
            currentKeyIndex = 0;
            GenerateKeySequence();
            ShowNextPrompt();
            StartCoroutine(TimeLimit());
        }
    }

    void GenerateKeySequence()
    {
        string[] possibleKeys = { "W", "A", "S", "D" };
        for (int i = 0; i < keySequence.Length; i++)
        {
            keySequence[i] = possibleKeys[Random.Range(0, possibleKeys.Length)];
        }
    }

    void ShowNextPrompt()
    {
        if (currentKeyIndex < keySequence.Length)
        {
            promptText.text = $"Press: {keySequence[currentKeyIndex]}";
        }
        else
        {
            EndQuickTimeEvent(true);
        }
    }

    void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.W) && keySequence[currentKeyIndex] == "W" ||
            Input.GetKeyDown(KeyCode.A) && keySequence[currentKeyIndex] == "A" ||
            Input.GetKeyDown(KeyCode.S) && keySequence[currentKeyIndex] == "S" ||
            Input.GetKeyDown(KeyCode.D) && keySequence[currentKeyIndex] == "D")
        {
            currentKeyIndex++;
            if (currentKeyIndex >= keySequence.Length)
            {
                EndQuickTimeEvent(true);
            }
            else
            {
                ShowNextPrompt();
            }
        } else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                 Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            EndQuickTimeEvent(false);
        }
    }

    IEnumerator TimeLimit()
    {
        yield return new WaitForSeconds(timePerKey * keySequence.Length);
        if (isActive)
        {
            EndQuickTimeEvent(false);
        }
    }

    void EndQuickTimeEvent(bool success)
    {
        isActive = false;
        promptText.text = "";

        if (success)
        {
            currentCow.OnSuccessfulRope();
        }
        else
        {
            currentCow.OnFailedRope();
        }

        currentCow = null;
    }
}