using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    //덱 셔플
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

    //덱 초기화
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
    //머지영역에서 카드 정렬 함수
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
            Debug.Log("카드 수가 최대 입니다. 공간을 확보하세요.");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("텍에 더 이상 카드가 없습니다.");
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

    public void MoveCardToMerge(GameObject card)  //머지 영역으로 이동
    {
        if (mergeCount >= maxMergeSize)   //머지영역이 가득 찼는지
        {
            Debug.Log("머지 영역이 가득 찼습니다.");
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

        card.transform.SetParent(mergeArea);  //머지 영역을 부모로 둔다.
        ArrangeHand();
        UpdateMergeButtonState();     //머지버튼상태 업데이트
    }

    //머지 버튼 상태 업데이트
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
            Debug.Log("머지를 하려면 카드가 2개 또는 3개가 필요합니다.");
            return;
        }
        int fristCardValue = mergeCards[0].GetComponent<Card>().cardValue;
        for (int i = 1; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if (card != null || card.cardValue != fristCardValue)
            {
                Debug.Log("같은 숫자의 카드만 머지 할 수 있습니다.");
                return;
            }
        }
        int newValue = fristCardValue + 1;

        if(newValue > cardImage.Length)
        {
            Debug.Log("최대 카드 값에 도달했습니다.");
            return ;
        }

        for(int i = 0; i < mergeCount; i++)         //머지 영역의 카드 비활성화
        {
            if (mergeCards[i] !=null)
            {
                mergeCards[i].SetActive(false);
            }
        }

        //새 카드 생성
        GameObject newCard = Instantiate(cardPrefabs, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();
        if(newCardTemp != null)
        {
            int imgeIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImage[imgeIndex]);
        }

        //머지 영역 비우기
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

    //머지 버튼 클릭시 머지 영역의 카드를 합성
    void OnMergeButtonClicked()
    {
        MergeCards();
    }
}


