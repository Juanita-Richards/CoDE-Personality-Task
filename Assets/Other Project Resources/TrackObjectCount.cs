using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.IO;
using System;
using UnityEngine;
using TMPro;

public class TrackObjectCount : MonoBehaviour
{

    private SnapScript[] snapScripts;
    private GameObject doorGhosts;
    private DoorLerpAnim[] doorLerpAnims;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI timer;
    private int total = 0;
    private int current = 0;
    private bool onFlag = false;
    public float timerCountUp, currentTime;
    public bool timeStarted;
    public GameObject PID;
    public GameObject PT;
    private string ParticipantID;
    public AudioSource closeDoors;

    readonly string log_path = "Participant_Constructive_log.csv";
   
   

    void Start()
    {
        //Collect the total of elements that has the snap script.
        total = GetScriptAmount(snapScripts);
        totalText.text = total.ToString();
        timer.text = timerCountUp.ToString();
        

    }

    private void Update()
    {

        //start Timer

        if (timeStarted)
        {
            StartCoroutine(TimeCount());
        }

        
       
        //start ghost doors
        if (onFlag == false)
        {
            //sets current and text
            current = total - GetScriptAmount(snapScripts);
            currentText.text = current.ToString();// setting text in UI

            if (current == total)
            {
                //activate ghost doors
                doorGhosts = GameObject.FindWithTag("DoorGhosts");
                doorLerpAnims = doorGhosts.GetComponentsInChildren<DoorLerpAnim>(); //get the objects with this script

                foreach (DoorLerpAnim door in doorLerpAnims) 
                {
                    //call functions from DoorLerpAnim script
                    door.ToggleMeshes(true);
                    door.StartLerpAnim(); 
                }

                //setting the door text and new total
                titleText.text = "Doors Left: ";
                total = GetDoorAmount(doorLerpAnims);
                totalText.text = total.ToString();

                onFlag = true; //set this so it doesnt run again
            }
        }
        else
        {
            current = total - GetDoorAmount(doorLerpAnims);
            currentText.text = current.ToString();
            closeDoors.Play();

            //this is disabled because it is being handled in TurnBoolOff
            if (current == total)
            {
                //turns the timer off
                TurnBoolOff();
                
            }

        }
        
    }
   
    public void TurnBoolOn() //to start the timer
    {
        timeStarted = true;
    }
    public void TurnBoolOff() //to stop the timer
    {
        timeStarted = false;
    }
    public void LogData()
    {
        LoadData();
       
    }
    void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60); //this converts the seconds into minutes
        float seconds = Mathf.FloorToInt(time % 60); // this converts to seconds

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator TimeCount()
    {
        timerCountUp += Time.deltaTime;
        

        DisplayTime(timerCountUp);

        yield return null;
    }

    



    private static int GetScriptAmount(SnapScript[] snapScripts)
    {
        int count = 0;
        snapScripts = FindObjectsOfType<SnapScript>();
        foreach (SnapScript script in snapScripts)
        {
            count += 1;
        }
        return count;
    }

    private static int GetDoorAmount(DoorLerpAnim[] doorlerpAnims)
    {
        int count = 0;
        doorlerpAnims = FindObjectsOfType<DoorLerpAnim>();
        foreach (DoorLerpAnim door in doorlerpAnims)
        {
            //gets all doors with lerp variable == true
            if (door.GetLerpVal())
                count += 1;
            
        }
        return count;
    }

    //log the data in the CSV file
    void LoadData()
    {
        string dateTime = DateTime.Now.ToString("MMddyy-HHmm");
        string ParticipantID = PID.GetComponent<TMP_InputField>().text;
        string PersonalityType = PT.GetComponent<TMP_InputField>().text;
        if (!File.Exists(log_path))
        {
            using StreamWriter writer = File.CreateText(log_path);
            writer.WriteLine("ParticipantID, Personality Type, TimeStamp, TimeStampFloat,Date");
            
            writer.WriteLine($"{ParticipantID},{PersonalityType},{timer.text},{timerCountUp}, {dateTime}");
            writer.Close();

        }

        else
        {
            using StreamWriter writer = File.AppendText(log_path);
            writer.WriteLine($"{ParticipantID},{PersonalityType},{timer.text}, {timerCountUp},{dateTime}");
            writer.Close();
        }





     
    }


}
