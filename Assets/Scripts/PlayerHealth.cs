using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currcntLives;

    public float invincibleTime = 1.0f;
    public bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currcntLives = maxLives;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            currcntLives--;
            Destroy(other.gameObject);


            if(currcntLives <= 00 )
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestarGame", 3.0f);

    }

    void RestarGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}