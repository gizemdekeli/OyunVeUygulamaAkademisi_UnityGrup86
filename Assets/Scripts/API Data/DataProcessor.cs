using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcessor : MonoBehaviour
{
    public void GetTimeDate()
    {
        TimeData timeData = APIHelper.GetDayLight();
        Debug.Log(timeData.isDayLightSavingsTime);
    }
}
