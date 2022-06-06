using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataProcessor : MonoBehaviour
{
    [System.Obsolete]
    private void Awake()
    {
        SendTimeRequest();
    }

    [System.Obsolete]
    public void SendTimeRequest()
    {
        StartCoroutine(GetTime());
    }

    [System.Obsolete]
    IEnumerator GetTime()
    {
        string url = "http://worldclockapi.com/api/jsonp/cet/now?callback=mycallback";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string sonText = request.downloadHandler.text;
                Debug.Log(sonText);
            }
        } ;
    }
}
