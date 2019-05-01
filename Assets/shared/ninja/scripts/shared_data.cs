using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class shared_data {
    private static float ninja_x = 0;
    private static float ninja_y = 0;
    private static int start_direction = 1;

    public static float Ninja_x
    {
        get
        {
            return ninja_x;
        }
        set
        {
            ninja_x = value;
        }
    }

    public static float Ninja_y
    {
        get
        {
            return ninja_y;
        }
        set
        {
            ninja_y = value;
        }
    }

    public static int Start_direction
    {
        get
        {
            return start_direction;
        }
        set
        {
            start_direction = value;
        }
    }
}
