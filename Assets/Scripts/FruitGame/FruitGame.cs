using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public int fruitType;                   //���� Ÿ�� (0 : ��� , 1 : ��纣�� , 2 : ���ڳ�)
    
    public bool hasMered = false;           //������ ���������� Ȯ���ϴ� �÷���

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hasMered)
            return;               //�̹� ������ ������ ����

        FruitGame otherFruit = collision.gameObject.GetComponent<FruitGame>();       //�ٸ� ���ϰ� �浿�ߴ��� Ȯ��

        if(otherFruit != null && !otherFruit.hasMered && otherFruit.fruitType == fruitType)     //�浹�� ���� �����̰� Ÿ���� ���ٸ�
        {
            hasMered = true;     //���ƴٰ� ǥ��
            otherFruit.hasMered = true;

            Vector3 mergePosion = (transform.position + otherFruit.transform.position) / 2f;      //�� ������ �߰� ��ġ ���


            //���� �Ŵ������� Merge ����
            FruitGameMaster gameManager = FindObjectOfType<FruitGameMaster>();
            if(gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosion);
            }


            //���ϵ� ����
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }


}
