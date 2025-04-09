using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CudeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalCube = 10;
    public float cubeSpacing = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position;

        GameObject firestCube = Instantiate(cubePrefab, myPosition, Quaternion.identity);

        for(int i = 1; i < totalCube; i++)
        {
            Vector3 Position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, Position, Quaternion.identity);
        }
    }
   
}
