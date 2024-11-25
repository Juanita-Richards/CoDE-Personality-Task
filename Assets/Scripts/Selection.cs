using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class Selection : MonoBehaviour
{
    public GameObject textPromptObject;
    public int selectionNum = 1;
    private Text textPrompt;
    public Participant participant;

    public GameObject AFAvatars;
    public GameObject AMAvatars;
    public GameObject BFAvatars;
    public GameObject BMAvatars;
    public GameObject HFAvatars;
    public GameObject HMAvatars;
    public GameObject WFAvatars;
    public GameObject WMAvatars;

    private List<GameObject> avatarList;
    private List<Vector3> positions;
    private Transform row1Position;
    private Transform row2Position;
    private Transform row3Position;
    private Transform row4Position;
    private GameObject preview;
    private GameObject headshotPreview;
    private Sprite originalUIImage;

    private readonly string log_path = "participant_log.csv";

    // Start is called before the first frame update
    void Start()
    {
        textPrompt = textPromptObject.GetComponent<Text>();
        row1Position = AFAvatars.GetComponent<Transform>();
        row2Position = BFAvatars.GetComponent<Transform>();
        row3Position = HFAvatars.GetComponent<Transform>();
        row4Position = WFAvatars.GetComponent<Transform>();

        avatarList = new();
        avatarList.Add(AFAvatars);
        avatarList.Add(AMAvatars);
        avatarList.Add(BFAvatars);
        avatarList.Add(BMAvatars);
        avatarList.Add(HFAvatars);
        avatarList.Add(HMAvatars);
        avatarList.Add(WFAvatars);
        avatarList.Add(WMAvatars);
        positions = new();
        positions.Add(new Vector3(row1Position.position.x, row1Position.position.y, row1Position.position.z));
        positions.Add(new Vector3(row2Position.position.x, row2Position.position.y, row2Position.position.z));
        positions.Add(new Vector3(row3Position.position.x, row3Position.position.y, row3Position.position.z));
        positions.Add(new Vector3(row4Position.position.x, row4Position.position.y, row4Position.position.z));
        preview = GameObject.Find("Preview");
        headshotPreview = GameObject.Find("HeadshotPreview");
        originalUIImage = preview.GetComponent<Image>().sprite;
        LoadParticipantDetails();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void LoadParticipantDetails()
    {
        string participantID = PlayerPrefs.GetString("participantID");
        if (File.Exists(log_path))
        {
            using StreamReader sr = File.OpenText(log_path);
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string[] ray = sr.ReadLine().Split(",");
                if (ray[0] == participantID)
                {
                    participant = new Participant(ray[0], ray[1], ray[2], ray[3], int.Parse(ray[4]));
                }
            }
        }
    }

    public void UpdateUI()
    {
        // reset UI
        foreach(GameObject avatar in avatarList)
        {
            avatar.SetActive(false);
        }
        // next is matched both ethnicity and gender
        if (selectionNum == 1)
        {
            if (participant.ParticipantRace == "A")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(AFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(AMAvatars, 0);
            }
            if (participant.ParticipantRace == "B")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BMAvatars, 0);
            }
            if (participant.ParticipantRace == "H")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(HFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(HMAvatars, 0);
            }
            if (participant.ParticipantRace == "W")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(WFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(WMAvatars, 0);
            }
            if (participant.ParticipantRace == "O")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                {
                    ShowAndRearrange(AFAvatars, 0);
                    ShowAndRearrange(BFAvatars, 1);
                    ShowAndRearrange(HFAvatars, 2);
                    ShowAndRearrange(WFAvatars, 3);
                }
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                {
                    ShowAndRearrange(AMAvatars, 0);
                    ShowAndRearrange(BMAvatars, 1);
                    ShowAndRearrange(HMAvatars, 2);
                    ShowAndRearrange(WMAvatars, 3);
                }
            }
        }
        // next is matched ethnicity, unmatched gender
        if (selectionNum == 2)
        {
            if (participant.ParticipantRace == "A")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(AMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(AFAvatars, 0);
            }
            if (participant.ParticipantRace == "B")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(BFAvatars, 0);
            }
            if (participant.ParticipantRace == "H")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(HMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(HFAvatars, 0);
            }
            if (participant.ParticipantRace == "W")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(WMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(WFAvatars, 0);
            }
            if (participant.ParticipantRace == "O")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                {
                    ShowAndRearrange(AMAvatars, 0);
                    ShowAndRearrange(BMAvatars, 1);
                    ShowAndRearrange(HMAvatars, 2);
                    ShowAndRearrange(WMAvatars, 3);
                }
                if (participant.ParticipantGender == "M")
                {
                    ShowAndRearrange(AFAvatars, 0);
                    ShowAndRearrange(BFAvatars, 1);
                    ShowAndRearrange(HFAvatars, 2);
                    ShowAndRearrange(WFAvatars, 3);
                }
            }
        }
        // next is matched gender, unmatched ethnicity
        if (selectionNum == 3)
        {
            if (participant.ParticipantRace == "A")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BMAvatars, 0);
            }
            if (participant.ParticipantRace == "B" || participant.ParticipantRace == "O")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(WFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(WMAvatars, 0);
            }
            if (participant.ParticipantRace == "H")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(AFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(AMAvatars, 0);
            }
            if (participant.ParticipantRace == "W")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BFAvatars, 0);
                if (participant.ParticipantGender == "M" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BMAvatars, 0);
            }
        }
        // next is unmatched gender, unmatched ethnicity
        if (selectionNum == 4)
        {
            if (participant.ParticipantRace == "A")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(BFAvatars, 0);
            }
            if (participant.ParticipantRace == "B" || participant.ParticipantRace == "O")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(WMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(WFAvatars, 0);
            }
            if (participant.ParticipantRace == "H")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(AMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(AFAvatars, 0);
            }
            if (participant.ParticipantRace == "W")
            {
                if (participant.ParticipantGender == "F" || participant.ParticipantGender == "O")
                    ShowAndRearrange(BMAvatars, 0);
                if (participant.ParticipantGender == "M")
                    ShowAndRearrange(BFAvatars, 0);
            }
        }
        
        // all selections have been made
        if (selectionNum == 5)
        {
            using StreamReader sr = File.OpenText(log_path);
            List<string> file = new();
            // find participant's row, and edit it
            if (File.Exists(log_path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] ray = line.Split(",");
                    if (ray[0] == participant.ParticipantID)
                        file.Add($"{participant.ParticipantID},{participant.ParticipantRace},{participant.ParticipantGender},{participant.Order},{participant.Trial},{participant.ChosenAvatar},{participant.mAllAvatar},{participant.mEthnicityOnlyAvatar},{participant.mGenderOnlyAvatar},{participant.mNoneAvatar},{DateTime.Now}");
                    else
                        file.Add(line);
                }
            }

            // rewrite the file
            sr.Close();
            using StreamWriter sw = File.CreateText(log_path);
            foreach (string fileLine in file)
            {
                sw.WriteLine(fileLine);
            }
            sw.Close();

            // hide windows 
            preview.SetActive(false);
            headshotPreview.SetActive(false);
            GameObject.Find("Confirm").SetActive(false);
            textPrompt.text = "All selections have been made.";

        }

        // reset preview windows
        preview.GetComponent<Image>().sprite = originalUIImage;
        headshotPreview.GetComponent<Image>().sprite = originalUIImage;
        selectionNum++;
    }

    // rearranges the grid 
    private void ShowAndRearrange(GameObject mObject, int row)
    {
        mObject.transform.position = new Vector3(mObject.transform.position.x, positions[row].y, mObject.transform.position.z);
        mObject.SetActive(true);
    }
}
