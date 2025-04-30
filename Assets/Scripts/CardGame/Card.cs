using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue;
    public Sprite cardmage;
    public TextMeshPro cardText;

    //ī�� ���� �ʱ�ȭ �Լ�
    public void InitCard(int value, Sprite image)    //ī������ �ʱ�ȭ �Լ�
    {
        cardValue = value;
        cardmage = image;

        //ī�� �̹��� ����
        GetComponent<SpriteRenderer>().sprite = image;   //�ش� �̹����� ī�忡 ǥ���Ѵ�.

        //ī�� �ؽ�Ʈ ������ �ִ� ���
        if(cardText != null)
        {
            cardText.text = cardValue.ToString();     //ī�� ���� ǥ���Ѵ�.
        }
    }
}
