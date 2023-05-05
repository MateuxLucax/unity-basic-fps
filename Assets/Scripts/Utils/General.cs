using UnityEngine;

namespace Utils
{
    public class General
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
}