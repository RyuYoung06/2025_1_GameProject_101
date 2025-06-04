using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI요소 - Inspector 에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    private DialogueDataSO curretDialgue;
    private int currentLineIndex = 0;
    private bool IsDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);

    }

    private void Update()
    {
        if(IsDialogueActive && Input.GetKeyUp(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        for(int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }


    private void CompleteTyping()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping=false;

        if(curretDialgue != null && currentLineIndex < curretDialgue.dialogueLines.Count)
        {
            dialogueText .text = curretDialgue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if(curretDialgue !=null && currentLineIndex < curretDialgue .dialogueLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine (typingCoroutine);
            }
        }
        string curretText = curretDialgue.dialogueLines [currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(curretText));
    }

    void EndDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine (typingCoroutine);
            typingCoroutine = null;
        }

        IsDialogueActive = false;
        isTyping = false ;
        DialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if(currentLineIndex >= curretDialgue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    public void HandleNextInput()
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if(!isTyping) 
        {
            ShowNextLine();
        }
    }

    public void SkipDialogue()
    {
        EndDialogue ();
    }

    public bool isDialogueActive()
    {
        return IsDialogueActive;
    }

    public void StarDialogue(DialogueDataSO dialogue)
    {
        if(dialogue == null || dialogue.dialogueLines.Count == 0) return;

        curretDialgue = dialogue;
        currentLineIndex = 0;
        IsDialogueActive = true;

        DialoguePanel.SetActive (true);
        characternameText.text = dialogue.characterName;

        if(characterImage != null )
        {
            if(dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;
            }
        }
        ShowCurrentLine();
    }
}
