using KalmanSimulation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MarineTrafficAPI : MonoBehaviour
{
    public Boid boid;
    public InputField inputAPI;
    public InputField inputID;
    public Dropdown inputType;
    public Button buttonStart;
    private void Start()
    {
        //StartCoroutine(GetData("f525521f401343f71a775caee6988733962ca102", IdType.MMSI, 258809000, 1));
        buttonStart.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(GetData(inputAPI.text, (IdType)inputType.value, int.Parse(inputID.text), 1));
        inputAPI.enabled = false;
        inputID.enabled = false;
        inputType.enabled = false;
        buttonStart.enabled = false;
    }

    /// <summary>
    /// {0} - API Key (40-character hexadecimal number)
    /// {1} - mmsi/imo/shipid (format: mmsi:integer)
    /// {2} - days/fromdate/todate (format: days:integer OR fromdate:YYYY-MM-DD HH:MM:SS)
    /// </summary>
    public static string url = @"https://services.marinetraffic.com/api/exportvesseltrack/{0}/v:2/{1}/{2}/period:hourly/protocol:jsono";
    public IEnumerator GetData(string apiKey, IdType idType, int id, DateTime date, bool isFrom)
    {
        string ids = "";
        switch (idType)
        {
            case IdType.MMSI:
                ids = "mmsi:" + id;
                break;
            case IdType.IMO:
                ids = "imo:" + id;
                break;
            case IdType.SHIPID:
                ids = "shipid:" + id;
                break;
        }
        string dates = date.ToString("yyyy-MM-dd HH:mm:ss");
        if (isFrom)
            return GetData(apiKey, ids, "fromdate:" + dates);
        else
            return GetData(apiKey, ids, "todate:" + dates);
    }
    public IEnumerator GetData(string apiKey, IdType idType, int id, int days)
    {
        string ids = "";
        switch (idType)
        {
            case IdType.MMSI:
                ids = "mmsi:" + id;
                break;
            case IdType.IMO:
                ids = "imo:" + id;
                break;
            case IdType.SHIPID:
                ids = "shipid:" + id;
                break;
        }
        return GetData(apiKey, ids, "days:" + days);
    }
    private IEnumerator GetData(string apiKey, string id, string date)
    {
        //using (WWW www = new WWW(string.Format(url, apiKey, id, date)))
        {
            yield return null;
            //yield return www;
            //var json = www.text;
            var json = "[{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"68\",\"LON\":\"9.676635\",\"LAT\":\"54.292380\",\"COURSE\":\"73\",\"HEADING\":\"73\",\"TIMESTAMP\":\"2018-12-04T12:21:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"82\",\"LON\":\"9.846975\",\"LAT\":\"54.363660\",\"COURSE\":\"91\",\"HEADING\":\"93\",\"TIMESTAMP\":\"2018-12-04T13:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"73\",\"LON\":\"10.036660\",\"LAT\":\"54.360610\",\"COURSE\":\"95\",\"HEADING\":\"97\",\"TIMESTAMP\":\"2018-12-04T14:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"1\",\"LON\":\"10.141630\",\"LAT\":\"54.365610\",\"COURSE\":\"260\",\"HEADING\":\"105\",\"TIMESTAMP\":\"2018-12-04T15:23:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"125\",\"LON\":\"10.235980\",\"LAT\":\"54.439620\",\"COURSE\":\"27\",\"HEADING\":\"28\",\"TIMESTAMP\":\"2018-12-04T16:23:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"136\",\"LON\":\"10.527780\",\"LAT\":\"54.540850\",\"COURSE\":\"67\",\"HEADING\":\"67\",\"TIMESTAMP\":\"2018-12-04T17:23:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"153\",\"LON\":\"10.936280\",\"LAT\":\"54.578170\",\"COURSE\":\"83\",\"HEADING\":\"83\",\"TIMESTAMP\":\"2018-12-04T18:23:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"163\",\"LON\":\"11.374660\",\"LAT\":\"54.525310\",\"COURSE\":\"114\",\"HEADING\":\"114\",\"TIMESTAMP\":\"2018-12-04T19:22:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"144\",\"LON\":\"11.772130\",\"LAT\":\"54.417710\",\"COURSE\":\"115\",\"HEADING\":\"118\",\"TIMESTAMP\":\"2018-12-04T20:22:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"133\",\"LON\":\"12.171460\",\"LAT\":\"54.412820\",\"COURSE\":\"51\",\"HEADING\":\"48\",\"TIMESTAMP\":\"2018-12-04T21:21:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"139\",\"LON\":\"12.348440\",\"LAT\":\"54.601670\",\"COURSE\":\"58\",\"HEADING\":\"61\",\"TIMESTAMP\":\"2018-12-04T22:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"131\",\"LON\":\"12.674470\",\"LAT\":\"54.724120\",\"COURSE\":\"57\",\"HEADING\":\"55\",\"TIMESTAMP\":\"2018-12-04T23:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"137\",\"LON\":\"13.032450\",\"LAT\":\"54.765110\",\"COURSE\":\"92\",\"HEADING\":\"92\",\"TIMESTAMP\":\"2018-12-05T00:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"134\",\"LON\":\"13.425230\",\"LAT\":\"54.755170\",\"COURSE\":\"92\",\"HEADING\":\"92\",\"TIMESTAMP\":\"2018-12-05T01:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"133\",\"LON\":\"13.795330\",\"LAT\":\"54.706470\",\"COURSE\":\"108\",\"HEADING\":\"109\",\"TIMESTAMP\":\"2018-12-05T02:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"132\",\"LON\":\"14.160440\",\"LAT\":\"54.636930\",\"COURSE\":\"108\",\"HEADING\":\"108\",\"TIMESTAMP\":\"2018-12-05T03:20:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"131\",\"LON\":\"14.710780\",\"LAT\":\"54.616070\",\"COURSE\":\"80\",\"HEADING\":\"79\",\"TIMESTAMP\":\"2018-12-05T04:50:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"128\",\"LON\":\"15.077780\",\"LAT\":\"54.649990\",\"COURSE\":\"83\",\"HEADING\":\"81\",\"TIMESTAMP\":\"2018-12-05T05:51:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"102\",\"LON\":\"15.413770\",\"LAT\":\"54.677270\",\"COURSE\":\"80\",\"HEADING\":\"79\",\"TIMESTAMP\":\"2018-12-05T06:55:00\",\"SHIP_ID\":\"3541902\"},{\"MMSI\":\"538005781\",\"STATUS\":\"0\",\"SPEED\":\"104\",\"LON\":\"15.709750\",\"LAT\":\"54.704330\",\"COURSE\":\"83\",\"HEADING\":\"80\",\"TIMESTAMP\":\"2018-12-05T07:55:00\",\"SHIP_ID\":\"3541902\"}]";
            List<MarineTrafficResponse> data = new List<MarineTrafficResponse>();
            MarineTrafficResponse tmp = null;
            int start = -1;
            int end = -1;
            if (!string.IsNullOrEmpty(json))
                if (json[0] == '[')
                {
                    for (int i = 1; i < json.Length; ++i)
                    {
                        switch (json[i])
                        {
                            case '{':
                                start = i;
                                break;
                            case '}':
                                end = i;
                                string s = json.Substring(start, end - start + 1);
                                tmp = JsonUtility.FromJson<MarineTrafficResponse>(s);
                                int a = s.IndexOf("\"TIMESTAMP\":") + 13;
                                int b = s.IndexOf('"', a);
                                tmp.TIMESTAMP = DateTime.Parse(s.Substring(a, b - a));
                                data.Add(tmp);
                                break;
                        }
                    }
                }
            //TODO: Set data and start simulation

            data.Sort((a, b) => a.TIMESTAMP.CompareTo(b.TIMESTAMP));
            boid.Kalman.GPSData = data;
            boid.Activate();

            //Class.data = data;
            Debug.Log(json);
            foreach (var d in data)
            {
                Debug.Log(d.ToString());
            }

        }
    }
}

public enum IdType
{
    MMSI = 0,
    IMO = 1,
    SHIPID = 2
}

public class MarineTrafficResponse
{
    public int MMSI;
    public int STATUS;
    public int SPEED;
    public double LON;
    public double LAT;
    public int COURSE;
    public int HEADING;
    public DateTime TIMESTAMP;
    public int SHIP_ID;

    public override string ToString()
    {
        return "MMSI:" + MMSI + ", STATUS:" + STATUS + ", SPEED:" + SPEED + ", LON:" + LON + ", LAT:" + LAT + ", COURSE:" + COURSE + ", HEADING:" + HEADING + ", TIMESTAMP:" + TIMESTAMP.ToString() + ", SHIP_ID:" + SHIP_ID;
    }
}
