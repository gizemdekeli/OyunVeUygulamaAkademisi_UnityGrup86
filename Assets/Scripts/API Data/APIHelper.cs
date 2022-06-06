using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;

public static class APIHelper
{
    public static TimeData GetDayLight()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://worldclockapi.com/api/jsonp/cet/now?callback=mycallback");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());

        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<TimeData>(json);
    }
}
