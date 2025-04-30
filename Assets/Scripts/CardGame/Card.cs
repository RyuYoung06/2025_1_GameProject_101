using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue;
    public Sprite cardmage;
    public TextMeshPro cardText;

    //카드 정보 초기화 함수
    public void InitCard(int value, Sprite image)    //카드정보 초기화 함수
    {
        cardValue = value;
        cardmage = image;

        //카드 이미지 설정
        GetComponent<SpriteRenderer>().sprite = image;   //해당 이미지를 카드에 표시한다.

        //카드 텍스트 설정이 있는 경우
        if(cardText != null)
        {
            cardText.text = cardValue.ToString();     //카드 값을 표시한다.
        }
    }
}
