using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LogJoints : MonoBehaviour
{
    GameObject playerAvatarRoot;
    List<GameObject> allJoints;
    private string logPath;
    private string participantID;
    private string avatarType;
    private bool isLogging = false;
    private StreamWriter sw;
    private StringBuilder sb;
    public float timeToWrite = 60.0f;
    private float timeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        // wait a bit, and then find the player root
        StartCoroutine(WaitAtStart());
        allJoints = new();
        sb = new StringBuilder();
        participantID = PlayerPrefs.GetString("participantID");
        avatarType = PlayerPrefs.GetString("avatarType");
        logPath = $"Joints/{participantID}/Joints_{participantID}_{avatarType}_{DateTime.Now.ToFileTimeUtc()}.csv";
        if (!Directory.Exists($"Joints/{participantID}"))
            Directory.CreateDirectory($"Joints/{participantID}");
        sw = new StreamWriter(logPath);
    }

    // this has to be done on Update and not LateUpdate since FinalIK has some weird things that cause it to not work
    private void Update()
    {
        if (isLogging)
        {
            timeElapsed += Time.deltaTime;
            sb.Append($"\n{DateTime.Now.ToUniversalTime()},{timeElapsed:0.0000},");
            for (int i = 0; i < allJoints.Count; i++)
            {
                Quaternion quat = allJoints[i].transform.rotation;
                float[] sixDs = SixDConversions.To6D(quat);
                sb.Append($"{allJoints[i].transform.position.x:0.0000},{allJoints[i].transform.position.y:0.0000},{allJoints[i].transform.position.z:0.0000},{allJoints[i].transform.rotation.x:0.0000},{allJoints[i].transform.rotation.y:0.0000},{allJoints[i].transform.rotation.z:0.0000},{allJoints[i].transform.rotation.w:0.0000},");
                sb.Append($"{sixDs[0]:0.0000},{sixDs[1]:0.0000},{sixDs[2]:0.0000},{sixDs[3]:0.0000},{sixDs[4]:0.0000},{sixDs[5]:0.0000},");
            }

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

    IEnumerator WaitAtStart()
    {
        yield return new WaitForSeconds(0.3f);
        playerAvatarRoot = GameObject.Find("Reference");
        // recursively get all children
        GetAllChildren(playerAvatarRoot);

        // write header
        string line = "Timestamp,TimeElapsed,";
        for (int i = 0; i < allJoints.Count; i++)
        {
            string name = allJoints[i].name;
            line += $"{name}_position_x,{name}_position_y,{name}_position_z,{name}_quat_x,{name}_quat_y,{name}_quat_z,{name}_quat_w,{name}_sixD_a,{name}_sixD_b,{name}_sixD_c,{name}_sixD_d,{name}_sixD_e,{name}_sixD_f,";
        }
        sw.Write(line);
    }

    private void GetAllChildren(GameObject obj)
    {
        if (obj == null)
            return;

        foreach (Transform child in obj.transform)
        {
            if (child == null || child.transform.name.EndsWith("_end"))
                continue;
            allJoints.Add(child.gameObject);
            GetAllChildren(child.gameObject);
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
        sw.Write(sb.ToString());
        StopLogging();
        sw.Close();
    }
}
