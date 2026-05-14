using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerScript : MonoBehaviour
{
    private DestructiveOutine[] destructiveOutlines;
    public int total = 0;
    private int current = 0;
    public TextMeshProUGUI totalText, currentText;
    public TextMeshProUGUI timer;
    public float timerCountUp;
    public float reset;
    public bool timeStarted;
    public GameObject PID;
    public GameObject PT;
    private string ParticipantID;

     string log_path = "Participant_Destructive_log.csv";

  

    // Start is called before the first frame update
    void Start()
    {
        total = GetOutlineAmount(destructiveOutlines);
        totalText.text = total.ToString();
        var currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);
        timer.text = timerCountUp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStarted)
        {
            StartCoroutine(TimeCount());
        }
        //tracking the current objects 
        current = total - GetOutlineAmount(destructiveOutlines);
        currentText.text = current.ToString();

        if (current == total)
        {
            TurnBoolOff();
        }

    }

    //Getting the amount of objects with the outline
    public static int GetOutlineAmount(DestructiveOutine[] destructiveOutlines)
    {
        int count = 0;
        destructiveOutlines = FindObjectsByType<DestructiveOutine>(FindObjectsSortMode.None);
        foreach (DestructiveOutine destructive in destructiveOutlines)
        {
            count += 1;
        }

        return count;

    }
    public void TurnBoolOff()
    {
        timeStarted = false;
       
    }

    public void LogData()
    {
        LoadData();
    }
    public void TurnBoolOn()
    {
        timeStarted = true;
    }
    void DisplayTime(float time)

    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator TimeCount()
    {
        // time countdown
        timerCountUp += Time.deltaTime;
   


        DisplayTime(timerCountUp);

        yield return null;
    }

    //will change to Personality Type

    void LoadData() //this function loads data into the csv file
    {
        string dateTime = DateTime.Now.ToString("MMddyy-HHmm");
        ParticipantID = PID.GetComponent<TMP_InputField>().text;
        string PersonalityType = PT.GetComponent<TMP_InputField>().text;
        if (!File.Exists(log_path)) //if the file does not exist, it will create one.
        {
            using StreamWriter writer = File.CreateText(log_path);
            writer.WriteLine("ParticipantID, Personality Type, TimeStamp, TimeStampFloat,Date");
            writer.WriteLine($"{ParticipantID},{PersonalityType},{timer.text}, {timerCountUp},{dateTime}");
            writer.Close();

        }

        else
        {
            using StreamWriter writer = File.AppendText(log_path);
            writer.WriteLine($"{ParticipantID},{PersonalityType},{timer.text},{timerCountUp}, {dateTime}");
            writer.Close();
        }

      
    
        
    }

    




}








