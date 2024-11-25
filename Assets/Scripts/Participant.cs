using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant
{
    public string ParticipantID { get; set; }
    public string ParticipantRace { get; set; }
    public string ParticipantGender { get; set; }
    public string Order { get; set; }
    public int Trial { get; set; }
    public string ChosenAvatar;
    public string mAllAvatar;
    public string mEthnicityOnlyAvatar;
    public string mGenderOnlyAvatar;
    public string mNoneAvatar;
    public Participant(string id, string race, string gender, string order, int trial)
    {
        ParticipantID = id;
        ParticipantRace = race;
        ParticipantGender = gender;
        Order = order;
        Trial = trial;
    }

    public Participant(string id, string race, string gender, string order, int trial, string chosenAvatar, string avatar1, string avatar2, string avatar3, string avatar4)
    {
        ParticipantID = id;
        ParticipantRace = race;
        ParticipantGender = gender;
        Order = order;
        Trial = trial;
        ChosenAvatar = chosenAvatar;
        mAllAvatar = avatar1;
        mEthnicityOnlyAvatar = avatar2;
        mGenderOnlyAvatar = avatar3;
        mNoneAvatar = avatar4;
    }

    public override string ToString() 
    {
        return $"{ParticipantID} {ParticipantRace}{ParticipantGender} Order: {Order} Trial:{Trial}\nA1:{ChosenAvatar} A2:{mAllAvatar} A3:{mEthnicityOnlyAvatar} A4:{mGenderOnlyAvatar} A5:{mNoneAvatar}";
    }
}
