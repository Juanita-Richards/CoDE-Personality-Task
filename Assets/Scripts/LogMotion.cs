using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LogMotion : MonoBehaviour
{
    public List<GameObject> TrackedObjects;
    private string participantID;
    private string avatarType;
    private bool isLogging = false;
    private MirrorRoth narrator;
    private float timeToWrite = 60.0f;

    // variables for motion tracking data
    private string logPath;
    private StreamWriter sw;
    private StringBuilder sb;
    private float timeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        sb = new StringBuilder();
        participantID = PlayerPrefs.GetString("participantID");
        //avatarType = PlayerPrefs.GetString("avatarType");
        //narrator = GameObject.Find("NarratorController").GetComponent<MirrorRoth>();

        // handles initial file creation for motion data logging 
        logPath = $"Motion/{participantID}/Motion_{participantID}_{avatarType}_{DateTime.Now.ToFileTimeUtc()}.csv";
        if (!Directory.Exists($"Motion/{participantID}"))
            Directory.CreateDirectory($"Motion/{participantID}");
        string line = "Timestamp,TimeElapsed,";
        for (int i = 0; i < TrackedObjects.Count; i++)
        {
            string name = TrackedObjects[i].name;
            line += $"{name}_position_x,{name}_position_y,{name}_position_z,{name}_quat_x,{name}_quat_y,{name}_quat_z,{name}_quat_w,{name}_sixD_a,{name}_sixD_b,{name}_sixD_c,{name}_sixD_d,{name}_sixD_e,{name}_sixD_f,";
        }
        line += "Task";
        sw = new StreamWriter(logPath);
        sw.WriteLine(line);

    }


    private void Update()
    {
        // handles logging tracked VR objects
        if (isLogging)
        {
            timeElapsed += Time.deltaTime;
            sb.Append($"\n{DateTime.Now.ToUniversalTime()},{timeElapsed:0.0000},");
            for (int i = 0; i < TrackedObjects.Count; i++)
            {
                Quaternion quat = TrackedObjects[i].transform.rotation;
                float[] sixDs = SixDConversions.To6D(quat);
                sb.Append($"{TrackedObjects[i].transform.position.x:0.0000},{TrackedObjects[i].transform.position.y:0.0000},{TrackedObjects[i].transform.position.z:0.0000},{TrackedObjects[i].transform.rotation.x:0.0000},{TrackedObjects[i].transform.rotation.y:0.0000},{TrackedObjects[i].transform.rotation.z:0.0000},{TrackedObjects[i].transform.rotation.w:0.0000},");
                sb.Append($"{sixDs[0]:0.0000},{sixDs[1]:0.0000},{sixDs[2]:0.0000},{sixDs[3]:0.0000},{sixDs[4]:0.0000},{sixDs[5]:0.0000},");
            }
            //sb.Append(31-narrator.currentVoiceLine);

            // write to disk evey minute
            timeToWrite -= Time.deltaTime;
            if (timeToWrite <= 0.0f)
            {
                sw.Write(sb.ToString());
                sb.Clear();
                timeToWrite = 60.0f;
            }
        }
    }

    
    public void StartLogging()
    {
        isLogging = true;
    }

    public void StopLogging()
    {
        isLogging = false;
    }

    private void OnApplicationQuit()
    {
        StopLogging();
        sw.Close();
    }
}
