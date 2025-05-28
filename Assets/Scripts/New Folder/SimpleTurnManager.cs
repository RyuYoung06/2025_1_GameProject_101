using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    public static bool canPlay = true;
    public static bool anyBallMoveing = false;

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();

        if(!anyBallMoveing && !canPlay)
        {
            canPlay = true ;
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�.");
        }
    }

    void CheckAllBalls()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoveing = false ;

        foreach(SimpleBallController ball  in allBalls)
        {
            if(ball.IsMoving())
            {
                anyBallMoveing = true ;
                break ;
            }
        }
    }

    public static void OnBallHit()
    {
        canPlay = false;
        anyBallMoveing = true;
        Debug.Log("�� ����! ���� ���� ������ ��ٸ�����.");
    }
}
