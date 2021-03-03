using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Vector2 RotateVector(Vector2 vector, int rotation)
    {
        vector.x += Mathf.Cos(rotation * Mathf.Deg2Rad) * vector.x;
        vector.y += Mathf.Sin(rotation * Mathf.Deg2Rad) * vector.y;
        vector.Normalize();

        return vector;
    }
}
