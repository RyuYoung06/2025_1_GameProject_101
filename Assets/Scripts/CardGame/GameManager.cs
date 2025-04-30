using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        1, 1, 1, 1,1, 1,1, 1,
        2, 2, 2, 2, 2, 2,
        3, 3, 3, 3,
        4, 4,
    };

    // Start is called before the first frame update
    void Start()
    {
        deckCards = new GameObject[prefadinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�� ����
    void ShuffleDeck()
    {
        for(int i = 0; i < deckCount - 1; i++)
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
         
        for(int i = 0; i < prefadinedDeck.Length; i++)
        {
            int value = prefadinedDeck[i];

            int imgaeIndex = value - 1;
            if(imgaeIndex >= cardImage.Length || imgaeIndex < 0)
            {
                imgaeIndex = 0;
            }

            GameObject newCardObj = Instantiate(cardPrefabs, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if(cardComp != null )
            {
                cardComp.InitCard(value, cardImage[imgaeIndex]);
            }
            deckCards[i] = newCardObj;
        }
    }

    public  void ArrangeHand()
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


    void OnDrawButtonClicked()
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()
    {
        if(handCount >= maxHandSize)
        {
            Debug.Log("���а� ���� á���ϴ�.");
            return;
        }
        if(deckCount <= 0)
        {
            Debug.Log("�ؿ� �� �̻� ī�尡 �����ϴ�.");
            return;
        }
        GameObject drawnCard = deckCards[0];

        for (int i = 0; i< deckCount -1; i++)
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

    //ī�带 ���з� �̵�
   // public void MoveCardToHand(GameObject card)


}
