using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.Burst.Intrinsics;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance => instance;

    private int _score = 0;

    public int score
    {
        get => _score;
        set
        {
            _score = value;
            Debug.Log("Score has been set to: " + _score.ToString());
            OnScoreValueChanged?.Invoke(_score);
        }
    }

    private int _health = 0;

    public int health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health > maxHealth)
            {
                _health = maxHealth;
            }

            if (_health <= 0)
            {
                SceneManager.LoadScene(2);
            }

            Debug.Log("Health has been set to: " + _health.ToString());
            OnHealthValueChanged?.Invoke(_health);
        }
    }
    public int maxHealth = 5;

    public UnityEvent<int> OnHealthValueChanged;
    public UnityEvent<int> OnScoreValueChanged;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}