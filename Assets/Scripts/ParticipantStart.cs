using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ParticipantStart : MonoBehaviour
{
    // fields for new participant
    public GameObject id_field;
    public GameObject race_field;
    public GameObject gender_field;
    public GameObject hidden_text;
    public GameObject warning_text;
    public bool newParticipant = true;

    // fields for returning participant
    public GameObject returning_dropdown;
    private List<Participant> participants = new();
    private Participant current_participant;
    private bool redoLast = false;

    private readonly string log_path = "participant_log.csv";

   

    private void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        if (newParticipant)
        {
            string participantID = id_field.GetComponent<TMP_InputField>().text;

            TMP_Dropdown race_dropDown = race_field.GetComponent<TMP_Dropdown>();
            string participantRace = race_dropDown.options[race_dropDown.value].text;

            TMP_Dropdown gender_dropDown = gender_field.GetComponent<TMP_Dropdown>();
            string participantGender = gender_dropDown.options[gender_dropDown.value].text;

            // assign latin square order (A,B,C,D)
            int participantNumber = 1;
            string order = "";
            foreach (Participant participant in participants)
            {
                if (participant.ParticipantRace == participantRace && participant.ParticipantGender == participantGender)
                    participantNumber += 1;
                if (participant.ParticipantID == participantID)
                {
                    Debug.Log("ID already exists");
                    warning_text.GetComponent<TMP_Text>().text = "ID already exists";
                    return;
                }
            }

            if (participantNumber % 4 == 1)
                order = "A";
            else if (participantNumber % 4 == 2)
                order = "B";
            else if (participantNumber % 4 == 3)
                order = "C";
            else if (participantNumber % 4 == 0)
                order = "D";

            int trial = 0;
            if (!File.Exists(log_path))
            {
                using StreamWriter sw = File.CreateText(log_path);
                sw.WriteLine("participantID,participantRace,participantGender,order,trial,ChosenAvatar,A,E,G,N,timestamp");
                sw.WriteLine($"{participantID},{participantRace},{participantGender},{order},{trial},None,None,None,None,None,{DateTime.Now}");
                sw.Close();
            }
            else
            {
                using StreamWriter sw = File.AppendText(log_path);
                sw.WriteLine($"{participantID},{participantRace},{participantGender},{order},{trial},None,None,None,None,None,{DateTime.Now}");
                sw.Close();
            }
            participants.Add(new Participant(participantID, participantRace, participantGender, order, trial));
            // start selection menu 
            PlayerPrefs.SetString("participantID", participantID);
            SceneManager.LoadScene("Selection2D");
        }
        else
        {
            using StreamReader sr = File.OpenText(log_path);
            List<string> file = new();

            if (redoLast && current_participant.Trial > 0)
                current_participant.Trial--;

            // find participant's row, and edit it
            if (File.Exists(log_path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] ray = line.Split(",");
                    if (ray[0] == current_participant.ParticipantID)
                        file.Add($"{current_participant.ParticipantID},{current_participant.ParticipantRace},{current_participant.ParticipantGender},{current_participant.Order},{current_participant.Trial+1},{current_participant.ChosenAvatar},{current_participant.mAllAvatar},{current_participant.mEthnicityOnlyAvatar},{current_participant.mGenderOnlyAvatar},{current_participant.mNoneAvatar},{DateTime.Now}");
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



            PlayerPrefs.SetString("participantID", current_participant.ParticipantID);
            string currentAvatar = "";
            string avatarType = "";

            List<string> orderA_type = new() { "A", "E", "G", "N" };
            List<string> orderB_type = new() { "E", "G", "N", "A" };
            List<string> orderC_type = new() { "G", "N", "A", "E" };
            List<string> orderD_type = new() { "N", "A", "E", "G" };

            List<string> orderA = new() { current_participant.mAllAvatar, current_participant.mEthnicityOnlyAvatar, current_participant.mGenderOnlyAvatar, current_participant.mNoneAvatar };
            List<string> orderB = new() { current_participant.mEthnicityOnlyAvatar, current_participant.mGenderOnlyAvatar, current_participant.mNoneAvatar, current_participant.mAllAvatar };
            List<string> orderC = new() { current_participant.mGenderOnlyAvatar, current_participant.mNoneAvatar, current_participant.mAllAvatar, current_participant.mEthnicityOnlyAvatar };
            List<string> orderD = new() { current_participant.mNoneAvatar, current_participant.mAllAvatar, current_participant.mEthnicityOnlyAvatar, current_participant.mGenderOnlyAvatar };

            if (current_participant.Order == "A")
            {
                currentAvatar = orderA[current_participant.Trial];
                avatarType = orderA_type[current_participant.Trial];
            }
            if (current_participant.Order == "B")
            {
                currentAvatar = orderB[current_participant.Trial];
                avatarType = orderB_type[current_participant.Trial];
            }
            if (current_participant.Order == "C")
            {
                currentAvatar = orderC[current_participant.Trial];
                avatarType = orderC_type[current_participant.Trial];
            }
            if (current_participant.Order == "D")
            {
                currentAvatar = orderD[current_participant.Trial];
                avatarType = orderD_type[current_participant.Trial];
            }


            PlayerPrefs.SetString("avatarName", currentAvatar);
            PlayerPrefs.SetString("avatarType", avatarType);
            SceneManager.LoadScene("MirrorCondition");
        }
    }

    public void LoadData()
    {
        if (File.Exists(log_path))
        {
            using StreamReader sr = File.OpenText(log_path);
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string[] ray = sr.ReadLine().Split(",");
                participants.Add(new Participant(ray[0], ray[1], ray[2], ray[3], int.Parse(ray[4]), ray[5], ray[6], ray[7], ray[8], ray[9]));
            }

            for (int i = participants.Count-1; i >= 0; i--)
            {
                returning_dropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData() { text = participants[i].ParticipantID});
            }
            sr.Close();
        }
    }

    public void ReturningSelected()
    {
        newParticipant = false;
        // set new participant fields to nothing
        id_field.GetComponent<TMP_InputField>().text = "";
        race_field.GetComponent<TMP_Dropdown>().value = 0;
        gender_field.GetComponent<TMP_Dropdown>().value = 0;


        // find the participant's data 
        TMP_Dropdown selected_dropdown = returning_dropdown.GetComponent<TMP_Dropdown>();
        string selected_id = selected_dropdown.options[selected_dropdown.value].text;
        foreach (Participant participant in participants)
        {
            if (participant.ParticipantID == selected_id)
            {
                current_participant = participant;
            }
        }

        hidden_text.GetComponent<TMP_Text>().text = $"{current_participant.ParticipantRace} {current_participant.ParticipantGender} | Order {current_participant.Order} | Trial {current_participant.Trial+1}";
    }

    public void NewSelected()
    {
        newParticipant = true;
        returning_dropdown.GetComponent<TMP_Dropdown>().value = 0;
        hidden_text.GetComponent<TMP_Text>().text = "";
    }

    public void ToggleRedo()
    {
        redoLast = !redoLast;
    }
}
