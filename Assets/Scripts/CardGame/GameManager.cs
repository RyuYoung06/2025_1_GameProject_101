using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //프리팹 리소스
    public GameObject cardPrefabs;          //카드 프리팹
    public Sprite[] cardImage;              //카드 이미지 배열
    //영역 Transfrom
    public Transform deckArea;             //덱 영역
    public Transform handArea;
    //UI요소
    public Button drawButton;
    public TextMeshProUGUI deckCountText;
    //설정 값
    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;

    //배열 선언
    public GameObject[] deckCards;
    public int deckCount;

    public GameObject[] handCards;
    public int handCount;

    //미리 정의된 덱 카드 목록
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

    //덱 셔플
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

    //덱 초기화
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
            Debug.Log("손패가 가득 찼습니다.");
            return;
        }
        if(deckCount <= 0)
        {
            Debug.Log("텍에 더 이상 카드가 없습니다.");
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

    //카드를 손패로 이동
   // public void MoveCardToHand(GameObject card)


}
