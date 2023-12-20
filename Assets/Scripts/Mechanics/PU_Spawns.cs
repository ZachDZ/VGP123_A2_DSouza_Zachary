using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Spawns : MonoBehaviour
{

    public int spawnNum;

    public Transform powerUp1;


    // Start is called before the first frame update
    void Start()
    {

        spawnNum = Random.Range(0, 2);

        switch (spawnNum)
        {
            case 1:
                GameObject.Instantiate(powerUp1, gameObject.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }

    }
}
