using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineTrafficAPI : MonoBehaviour
{
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
        using (WWW www = new WWW(string.Format(url, apiKey, id, date)))
        {
            yield return www;
            var json = www.text;
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
    MMSI,
    IMO,
    SHIPID
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