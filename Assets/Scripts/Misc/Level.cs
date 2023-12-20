using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int StartingLives = 1;
    public int StartingScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.health = StartingLives;
        GameManager.Instance.score = StartingScore;
    }
}
