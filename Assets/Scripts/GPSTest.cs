using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;


public class GPSTest : MonoBehaviour
{
    public TMP_Text gpsLocation;
    public TMP_Text debug;
    public float interval;
    public float timer;

    public string locationName;

    private void Start()
    {
        StartCoroutine(StartGPS());
        StartCoroutine(Get());
        //gpsLocation.text = "N" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + " E" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude);
        //gpsLocation.text = "N" + UnitChange.DegreesConvertToDegreesMinutesSeconds(22.28871f) + " E" + UnitChange.DegreesConvertToDegreesMinutesSeconds(114.1945f);


    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > interval)
        {
            StartCoroutine(StartGPS());
            timer = 0;
        }

    }

    IEnumerator Get()
    {
#if UNITY_EDITOR
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/maps/place/" + "22°17'19.356\"N+114°11'40.2\"E");
        //debug.text = "https://www.google.com/maps/place/" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + "N" + "+" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude) + "E";

#elif PLATFORM_ANDROID
        //debug.text = "https://www.google.com/maps/place/" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + "N" + "+" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude) + "E";
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/maps/place/" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + "N" + "+" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude) + "E");
#endif
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }


        //Show results as text
        //Debug.Log(www.downloadHandler.text);
        locationName = GetLocationName(www.downloadHandler.text);
        Debug.Log(locationName);
        gpsLocation.text = locationName;
        // Or retrieve results as binary data
        byte[] results = www.downloadHandler.data;


    }

    private string GetLocationName(string html)
    {

        string[] ss = html.Split("<meta content=");
        string[] sss = ss[14].Split("\"");

        //foreach (var s in sss)
        //{
        //    Debug.Log(s);
        //}

        return sss[1];
    }

    IEnumerator StartGPS()
    {
        if (!Input.location.isEnabledByUser)
        {
            gpsLocation.text = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
            yield return false;
        }

        Input.location.Start(0.1f, 0.1f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            gpsLocation.text = "Init GPS service time out";
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gpsLocation.text = "Unable to determine device location";
            yield return false;
        }
        else
        {
            //gpsLocation.text = "N" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + " E" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude);
            //gpsLocation.text = "N" + UnitChange.DegreesConvertToDegreesMinutesSeconds(22.28871f) + " E" + UnitChange.DegreesConvertToDegreesMinutesSeconds(114.1945f);
            StartCoroutine(Get());
            gpsLocation.text = locationName;

            yield return new WaitForSeconds(100);
        }

    }

    public void ShowExactLocation()
    {
        // https://www.google.com/maps/place/22%C2%B017'19.4%22N+114%C2%B011'40.2%22E
        // 22°17'19.356"N 114°11'40.2"E
        Debug.Log("https://www.google.com/maps/place/" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + "N" + "+" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude) + "E");
#if UNITY_EDITOR
        Application.OpenURL("https://www.google.com/maps/place/" + "22°17'19.356\"N+114°11'40.2\"E");
#elif PLATFORM_ANDROID
        Application.OpenURL("https://www.google.com/maps/place/" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.latitude) + "N" + "+" + UnitChange.DegreesConvertToDegreesMinutesSeconds(Input.location.lastData.longitude) + "E");
#endif
    }




}
