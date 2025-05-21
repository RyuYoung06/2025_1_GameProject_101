using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public int fruitType;                   //과입 타입 (0 : 사과 , 1 : 블루베리 , 2 : 코코넛)
    
    public bool hasMered = false;           //과일이 합쳐졌는지 확인하는 플래그

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hasMered)
            return;               //이미 합쳐진 과일은 무시

        FruitGame otherFruit = collision.gameObject.GetComponent<FruitGame>();       //다른 과일과 충동했는지 확인

        if(otherFruit != null && !otherFruit.hasMered && otherFruit.fruitType == fruitType)     //충돌한 것이 과일이고 타일이 같다면
        {
            hasMered = true;     //합쳤다고 표시
            otherFruit.hasMered = true;

            Vector3 mergePosion = (transform.position + otherFruit.transform.position) / 2f;      //두 과일의 중간 위치 계산


            //게임 매니저에서 Merge 구현
            FruitGameMaster gameManager = FindObjectOfType<FruitGameMaster>();
            if(gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosion);
            }


            //과일들 제거
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }


}
