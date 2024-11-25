using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAvatar : MonoBehaviour
{
    string selectedAvatar = null;
    public List<GameObject> avatarPrefabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // get ID and other important things
        string participantID = PlayerPrefs.GetString("participantID");
        

        // get current avatar to display for this trial
        selectedAvatar = PlayerPrefs.GetString("avatarName");
        Avatar a = (Avatar)Enum.Parse(typeof(Avatar), selectedAvatar);
        Instantiate(avatarPrefabs[(int)a], new Vector3(0, -0.005f, 0), Quaternion.identity);
    }

    public enum Avatar 
    {
        Asian_F_1_Casual,
        Asian_F_2_Casual,
        Asian_F_3_Casual,
        Asian_M_1_Casual,
        Asian_M_2_Casual,
        Asian_M_3_Casual,
        Black_F_1_Casual,
        Black_F_2_Casual,
        Black_F_3_Casual,
        Black_M_1_Casual,
        Black_M_2_Casual,
        Black_M_3_Casual,
        Hispanic_F_1_Casual,
        Hispanic_F_2_Casual,
        Hispanic_F_3_Casual,
        Hispanic_M_1_Casual,
        Hispanic_M_2_Casual,
        Hispanic_M_3_Casual,
        White_F_1_Casual,
        White_F_2_Casual,
        White_F_3_Casual,
        White_M_1_Casual,
        White_M_2_Casual,
        White_M_3_Casual
    }

}
