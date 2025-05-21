using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGameMaster : MonoBehaviour
{
    public GameObject[] fruitPrefabs;         //과일 프리팹

    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };

    public GameObject currentFruit;
    public int currenFruitType;

    public float fruitStartHeight = 6.0f;
    public float gameWidth = 5.0f;
    public bool isGameOver = false;
    public Camera mainCamera;

    public float fruitTimer;

    public float gameHeight;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera =Camera.main;
        SpawnNewFruit();
        fruitTimer = -3.0f;
        gameHeight = fruitStartHeight + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if(fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;
        }

        if ( fruitTimer < 0  && fruitTimer > -2)
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;
        }

         
      



        if (currentFruit != null)
        {
            Vector3 mousePositon = Input.mousePosition;
            Vector3 worldPositon = mainCamera.ScreenToWorldPoint(mousePositon);

            Vector3 newPositon = currentFruit.transform.position;
            newPositon.x = worldPositon.x;

            float halfFruitSize = fruitSize[currenFruitType] / 2f;
            if (newPositon.x < -gameWidth / 2 + halfFruitSize)
            {
                newPositon.x = -gameWidth /2 + halfFruitSize;
            }
            if (newPositon.x > gameWidth /2 + halfFruitSize)
            {
                newPositon.x = gameWidth /2 + halfFruitSize;
            }

            currentFruit.transform.position = newPositon;
        }
        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
        }

    }

    void SpawnNewFruit()  //과일 생성합수
    {
        if(!isGameOver)     //게임오버가 아닐때
        {
            currenFruitType = Random.Range(0, 3);   //0~2사이의 랜덤

            Vector3 mousePosition =Input.mousePosition;
            Vector3 worldPosition =mainCamera.ScreenToViewportPoint(mousePosition);

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);

            float halfFruitSize = fruitSize[currenFruitType] / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currenFruitType], spawnPosition, Quaternion.identity);
            currentFruit.transform.localScale = new Vector3(fruitSize[currenFruitType], fruitSize[currenFruitType], 1);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.gravityScale = 0f;
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D> ();
        if(rb != null)
        {
            rb.gravityScale = 1f;

            currentFruit = null;

            fruitTimer = 1.0f;

        }
    }

    public void MergeFruits(int fruitType, Vector3 positon)
    {
        if(fruitType < fruitPrefabs.Length - 1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType +1], positon, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSize[fruitType + 1], fruitSize[fruitType + 1], 1.0f);
        }
    }

    public void CheckGameOver()
    {
        FruitGame[] allFruit = FindObjectsOfType<FruitGame>();

        float gameOverHeight = gameHeight;

        for(int i = 0; i < allFruit.Length; i++)
        {
            if(allFruit[i] != null)
            {
                Rigidbody2D rb = allFruit[i].GetComponent<Rigidbody2D> ();

                if(rb != null && rb.velocity.magnitude < 0.1f && allFruit[i].transform.position.y > gameOverHeight)
                {
                    isGameOver = true;
                    Debug.Log("게임오버");

                    break;
                }
            }
        }
    }
}
