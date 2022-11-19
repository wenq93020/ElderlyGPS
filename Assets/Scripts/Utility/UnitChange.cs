using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class UnitChange
{
    /// <summary>
    /// 度转度分秒
    /// </summary>
    /// <param name="latitude">纬度</param>
    /// <param name="londitude">经度</param>
    /// <returns></returns>
    public static string DegreesConvertToDegreesMinutesSeconds(float degrees)
    {
        if(degrees != 0)
        {
            string[] degreesString;
            string finalDegrees;
            degreesString = degrees.ToString().Split(".");
            degreesString[1] = "0." + degreesString[1];

            float secondDegreesFloat = Convert.ToSingle(degreesString[1]) * 60;
            float finalDegreesFloat = Convert.ToSingle("0." + secondDegreesFloat.ToString().Split(".")[1]) * 60;

            finalDegrees = degreesString[0] + "°" + secondDegreesFloat.ToString().Split(".")[0] + "'" + finalDegreesFloat + "\"";
            Debug.Log(finalDegrees);
            return finalDegrees;
        }
        return 0.ToString();

    }
}
