using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public EntityStatistics _statistics;
    public int money;

    void Awake()
    {
        instance = this;
    }
}