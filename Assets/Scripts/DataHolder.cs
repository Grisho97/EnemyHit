using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    private static int _Lives = -1;

    public static int Lives
    {
        get
        {
            return _Lives;
        }
        set
        {
            _Lives = value;
        }
    }
}
