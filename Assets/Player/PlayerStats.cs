using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    private static int health, maxHealth;

    public static int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public static int MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            maxHealth = value;
        }
    }
}
