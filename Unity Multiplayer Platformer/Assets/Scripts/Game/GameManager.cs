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

    // Not rotating vectors properly, needs fix
    public Vector2 RotateVector(Vector2 vector, int rotation)
    {
        Vector2 retVal = Vector2.zero;
        float radians = rotation * Mathf.PI / 180f;

        retVal.x = vector.x * Mathf.Cos(radians) + vector.y * -Mathf.Sin(radians);
        retVal.y = vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians);

        return retVal;
    }
}
