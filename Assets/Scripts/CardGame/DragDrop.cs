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
            Debug.Log("���� �������� �̵�");

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
                gameManager.ArrangeHand();  //�̹� ���п� �ִ� ī����  ���ļ���
            }
        }
        else if(IsOverArea(gameManager.mergeArea))         //���� �������� ī�带 ���Ҵ��� Ȯ��
        {
            if(gameManager.mergeCount >= gameManager.maxMergeSize)
            {
                Debug.Log("���� ������ ���� á���ϴ�.");
                RetrunToOtiginalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);
            }
        }
        else
        {
            RetrunToOtiginalPosition(); //�ƹ������� �ƴϸ� ���� ��ġ�� ���ư���
        }

        if(wasInMergeArea) //���� ������ ������� ��Ʈ ���� ������Ʈ
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
                Debug.Log("���� ������");
                return true;
            }

        }
        return false;
    }


}
