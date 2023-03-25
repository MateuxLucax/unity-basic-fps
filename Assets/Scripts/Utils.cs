using UnityEngine;

public class Utils
{
    public static bool IsNullOrDestroyed(object obj)
    {
        return obj switch
        {
            null => true,
            Object o => o == null,
            _ => false
        };
    }

}