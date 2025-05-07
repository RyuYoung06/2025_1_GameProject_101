using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //������ ���ҽ�
    public GameObject cardPrefabs;          //ī�� ������
    public Sprite[] cardImage;              //ī�� �̹��� �迭
    //���� Transfrom
    public Transform deckArea;             //�� ����
    public Transform handArea;
    //UI���
    public Button drawButton;
    public TextMeshProUGUI deckCountText;
    //���� ��
    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;

    //�迭 ����
    public GameObject[] deckCards;
    public int deckCount;

    public GameObject[] handCards;
    public int handCount;

    //�̸� ���ǵ� �� ī�� ���
    public int[] prefadinedDeck = new int[]
    {
        1, 1, 1, 1, 1, 1,1, 1,
        2, 2, 2, 2, 2, 2,
        3, 3, 3, 3,
        4, 4,
    };

    public Transform mergeArea;
    public Button mergeButton;
    public int maxMergeSize = 13;

    public GameObject[] mergeCards;
    public int mergeCount;

    // Start is called before the first frame update
    void Start()
    {
        deckCards = new GameObject[prefadinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        InitializeDeck();
        ShuffleDeck();

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);
        }

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OnMergeButtonClicked);
            mergeButton.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //�� ����
    void ShuffleDeck()
    {
        for (int i = 0; i < deckCount - 1; i++)
        {
            int j = Random.Range(i, deckCount);

            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }


    }

    //�� �ʱ�ȭ
    void InitializeDeck()
    {
        deckCount = prefadinedDeck.Length;

        for (int i = 0; i < prefadinedDeck.Length; i++)
        {
            int value = prefadinedDeck[i];

            int imgaeIndex = value - 1;
            if (imgaeIndex >= cardImage.Length || imgaeIndex < 0)
            {
                imgaeIndex = 0;
            }

            GameObject newCardObj = Instantiate(cardPrefabs, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.InitCard(value, cardImage[imgaeIndex]);
            }
            deckCards[i] = newCardObj;
        }
    }

    public void ArrangeHand()
    {
        if (handCount == 0)
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;

        for (int i = 0; i < handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }
        }
    }
    //������������ ī�� ���� �Լ�
    public void ArrangeMerge()
    {
        if (mergeCount == 0)
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos;
            }
        }
    }


    void OnDrawButtonClicked()
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()
    {
        if (handCount + mergeCount >= maxHandSize)
        {
            Debug.Log("ī�� ���� �ִ� �Դϴ�. ������ Ȯ���ϼ���.");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("�ؿ� �� �̻� ī�尡 �����ϴ�.");
            return;
        }
        GameObject drawnCard = deckCards[0];

        for (int i = 0; i < deckCount - 1; i++)
        {
            deckCards[i] = deckCards[i + 1];

        }
        deckCount--;

        drawnCard.SetActive(true);
        handCards[handCount] = drawnCard;
        handCount++;

        drawnCard.transform.SetParent(handArea);

        ArrangeHand();
    }

    public void MoveCardToMerge(GameObject card)  //���� �������� �̵�
    {
        if (mergeCount >= maxMergeSize)   //���������� ���� á����
        {
            Debug.Log("���� ������ ���� á���ϴ�.");
            return;
        }

        for (int i = 0; i < handCount; i++)
        {
            if (handCards[i] == card)
            {
                for(int j = i; j < handCount - 1; j++)
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;
                handCount--;

                ArrangeHand();
                break;
            }
        }

        mergeCards[mergeCount] = card;
        mergeCount++;

        card.transform.SetParent(mergeArea);  //���� ������ �θ�� �д�.
        ArrangeHand();
        UpdateMergeButtonState();     //������ư���� ������Ʈ
    }

    //���� ��ư ���� ������Ʈ
    void UpdateMergeButtonState()
    {
        if (mergeButton != null)
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);
        }
    }

    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("������ �Ϸ��� ī�尡 2�� �Ǵ� 3���� �ʿ��մϴ�.");
            return;
        }
        int fristCardValue = mergeCards[0].GetComponent<Card>().cardValue;
        for (int i = 1; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if (card != null || card.cardValue != fristCardValue)
            {
                Debug.Log("���� ������ ī�常 ���� �� �� �ֽ��ϴ�.");
                return;
            }
        }
        int newValue = fristCardValue + 1;

        if(newValue > cardImage.Length)
        {
            Debug.Log("�ִ� ī�� ���� �����߽��ϴ�.");
            return ;
        }

        for(int i = 0; i < mergeCount; i++)         //���� ������ ī�� ��Ȱ��ȭ
        {
            if (mergeCards[i] !=null)
            {
                mergeCards[i].SetActive(false);
            }
        }

        //�� ī�� ����
        GameObject newCard = Instantiate(cardPrefabs, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();
        if(newCardTemp != null)
        {
            int imgeIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImage[imgeIndex]);
        }

        //���� ���� ����
        for(int i = 0;i < maxMergeSize; i ++)
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;

        UpdateMergeButtonState();

        handCards[handCount] = newCard;
        handCount++;
        newCard.transform.SetParent(handArea);

        ArrangeHand();

    }

    //���� ��ư Ŭ���� ���� ������ ī�带 �ռ�
    void OnMergeButtonClicked()
    {
        MergeCards();
    }
}


