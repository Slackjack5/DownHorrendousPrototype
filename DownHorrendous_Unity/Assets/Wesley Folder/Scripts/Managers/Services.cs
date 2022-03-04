using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Services
{
    public static LevelManager levelManager;
    public static LevelEventManager levelEventManager;
    public static List<Lover> lovers = new List<Lover>(); //list in case more lovers get added/removed over the course of the level

    public static void Initialize()
    {
        levelEventManager = new LevelEventManager();
    }
}