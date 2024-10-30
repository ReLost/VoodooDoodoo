using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VoodooDoodoo.Utils
{
    public static class VoodooDoodooExtensions
    {
       public static List<T> Shuffle<T> (this List<T> list)
       {
           List<T> shuffledList = new(list);
           
           for (int i = shuffledList.Count - 1; i > 0; i--)
           {
               int j = Random.Range(0, i + 1);
               (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
           }

           return shuffledList;
       }
       
       public static Vector3 Parabola (Vector3 start, Vector3 end, float height, float t)
       {
           Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
           Vector3 mid = Vector3.Lerp(start, end, t);

           return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
       }
       
    
    }
}
