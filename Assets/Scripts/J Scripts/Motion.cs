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
            //I changed from Tracking to New_Tracking - 11.26.25
            log_path = $"New_Tracking/{currentScene.name}/{ParticipantID}_{Personality}_{DateTime.Now.ToString("MMddyy-HHmm")}.csv";
            if (!Directory.Exists($"New_Tracking/{currentScene.name}"))
            {
                Directory.CreateDirectory($"New_Tracking/{currentScene.name}");
            }
            //string headers = "Timestamp, TimeElapsed,"; //placeholder for now
            string headers = "TimeStamp,";
            for (int i = 0; i < MotionObjects.Count; i++)
            {
                string objName = MotionObjects[i].name;
                string label = "Label";
                headers += $"{objName}_position_x,{objName}_position_y,{objName}_position_z, {objName}_quat_x,{objName}_quat_y,{objName}_quat_z,{objName}_quat_w,{objName}_six_a,{objName}_six_b,{objName}_six_c,{objName}_six_d, {objName}_Six_e, {objName}_Six_f, {label}";

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
            //Add Label definition here
            var currentScene = SceneManager.GetActiveScene();
            var gender = PID.GetComponent<TMP_InputField>().text;
            var personality = PT.GetComponent<TMP_InputField>().text;

            timeElapsed += Time.deltaTime;
            //stringBuilder.Append($"\n{DateTime.Now.ToUniversalTime()}, {timeElapsed:0.0000},");
            stringBuilder.Append($"{timeElapsed:0.0000},"); //Using this for ML format 11.26.25
            for(int i = 0; i < MotionObjects.Count; i++)
            {
                Quaternion quaternion = MotionObjects[i].transform.rotation;
                float[] sixDegrees = SixDConversions.To6D(quaternion); //convert the Quaternion values to 6DoF
                stringBuilder.Append($"{MotionObjects[i].transform.position.x:0.0000},{MotionObjects[i].transform.position.y:0.0000},{MotionObjects[i].transform.position.z:0.0000},{MotionObjects[i].transform.rotation.x:0.0000}, {MotionObjects[i].transform.rotation.y:0.0000}, {MotionObjects[i].transform.rotation.z:0.0000},{MotionObjects[i].transform.rotation.w:0.0000},");
                stringBuilder.Append($"{sixDegrees[0]:0.0000}, {sixDegrees[1]:0.0000},{sixDegrees[2]:0.0000},{sixDegrees[3]:0.0000}, {sixDegrees[4]:0.0000}, {sixDegrees[5]:0.0000},");
                //Debug.Log(MotionObjects[i].transform.position.x);

                //stringBuilder.Append($"{personality} | {currentScene.name.Substring(0,1)} | {gender.Substring(10, 1)}");
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
