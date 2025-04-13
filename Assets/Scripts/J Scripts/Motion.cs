using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;
using TMPro;

public class Motion : MonoBehaviour
{
    //public TimerScript DestructiveScence;
    public List<GameObject> MotionObjects;
    private string ParticipantID;
    private string Personality;
  
   
    public GameObject PID, PT;

    private StreamWriter writer;
    private StringBuilder stringBuilder;
    private string log_path;
    public bool startTracking;
    private float writingTime = 60.0f;
    private float timeElapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        stringBuilder = new StringBuilder();
       
    }
    public void StartTrackingOn()
    {
        startTracking = true;


        if (startTracking)
        {
            //handles the creation of the motion tracking for each object
            var currentScene = SceneManager.GetActiveScene();
            ParticipantID = PID.GetComponent<TMP_InputField>().text;
            Personality = PT.GetComponent<TMP_InputField>().text;
            log_path = $"Tracking/{currentScene.name}/{ParticipantID}_{Personality}_{DateTime.Now.ToString("MMddyy-HHmm")}.csv";
            if (!Directory.Exists($"Tracking/{currentScene.name}"))
            {
                Directory.CreateDirectory($"Tracking/{currentScene.name}");
            }
            string headers = "Timestamp, TimeElapsed,"; //placeholder for now
            for (int i = 0; i < MotionObjects.Count; i++)
            {
                string objName = MotionObjects[i].name;
                headers += $"{objName}_Pos(x),{objName}_Pos(y),{objName}_Pos(z),{objName}_Quat_x,{objName}_Quat_y,{objName}_Quat_z,{objName}_Quat_w,{objName}_Six_x,{objName}_Six_y,{objName}_Six_z,{objName}_Six_a, {objName}_Six_b, {objName}_Six_c,";

            }

            //this will add the headers to the .csv file
            writer = new StreamWriter(log_path);
            writer.WriteLine(headers);

        }

        Debug.Log("tracking started");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startTracking)
        {

            timeElapsed += Time.deltaTime;
            stringBuilder.Append($"\n{DateTime.Now.ToUniversalTime()}, {timeElapsed:0.0000},");
            for(int i = 0; i < MotionObjects.Count; i++)
            {
                Quaternion quaternion = MotionObjects[i].transform.rotation;
                float[] sixDegrees = SixDConversions.To6D(quaternion); //convert the Quaternion values to 6DoF
                stringBuilder.Append($"{MotionObjects[i].transform.position.x:0.0000},{MotionObjects[i].transform.position.y:0.0000},{MotionObjects[i].transform.position.z:0.0000},{MotionObjects[i].transform.rotation.x:0.0000}, {MotionObjects[i].transform.rotation.y:0.0000}, {MotionObjects[i].transform.rotation.z:0.0000},{MotionObjects[i].transform.rotation.w:0.0000},");
                stringBuilder.Append($"{sixDegrees[0]:0.0000}, {sixDegrees[1]:0.0000},{sixDegrees[2]:0.0000},{sixDegrees[3]:0.0000}, {sixDegrees[4]:0.0000}, {sixDegrees[5]:0.0000},");
                //Debug.Log(MotionObjects[i].transform.position.x);
            }
            
        }
        //writing to the disk 
        writingTime -= Time.deltaTime;
        if (writingTime <= 0.0f)
        {
            writer.Write(stringBuilder.ToString());
            stringBuilder.Clear();
            writingTime = 60.0f;
        }
    }

    
    public void StopLoggingOff()
    {
        //turn the logging off
        startTracking = false;
        Debug.Log("tracking stopped");
        writer.Close();
    }

    //private void OnApplicationQuit()
    //{
    //    //close the application
    //    writer.Close();
    //}


}
