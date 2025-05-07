using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false;
    public Vector3 startPosition;
    public Transform startParent;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;

        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    void OnMouseUp()
    {
        isDragging= false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;

        if(gameManager == null)
        {
            RetrunToOtiginalPosition();
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;

        if(IsOverArea(gameManager.handArea))
        {
            Debug.Log("손패 영역으로 이동");

            if(wasInMergeArea)
            {
                for(int i = 0; i < gameManager.mergeCount; i++)
                {
                    if(gameManager.mergeCards[i] == gameObject)
                    {
                        for(int j = i; j < gameManager.mergeCount -1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j +1];
                        }
                        gameManager.mergeCards[gameManager.mergeCount -1] = null;
                        gameManager.mergeCount--;

                        transform.SetParent(gameManager.handArea);
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand();
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else
            {
                gameManager.ArrangeHand();  //이미 손패에 있는 카드라면  정렬수행
            }
        }
        else if(IsOverArea(gameManager.mergeArea))         //머지 영역위에 카드를 놓았는지 확인
        {
            if(gameManager.mergeCount >= gameManager.maxMergeSize)
            {
                Debug.Log("머지 영역이 가득 찼습니다.");
                RetrunToOtiginalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);
            }
        }
        else
        {
            RetrunToOtiginalPosition(); //아무영역도 아니면 원래 위치로 돌아가기
        }

        if(wasInMergeArea) //머지 영역에 있을경우 버트 선택 업데이트
        {
            if(gameManager.mergeButton != null)
            {
                bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                gameManager.mergeButton.interactable = canMerge;
            }
        }
      
    }

    void RetrunToOtiginalPosition()
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        if(gameManager != null)
        {
            if (startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeHand();
            }
        }

      
    }

    bool IsOverArea(Transform area)
    {
        if (area == null)
        {
            return false;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        foreach(RaycastHit2D hit in hits )
        {
            if( hit.collider != null && hit.collider.transform == area)
            {
                Debug.Log("영역 감지됨");
                return true;
            }

        }
        return false;
    }


}
