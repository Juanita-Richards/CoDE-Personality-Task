using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParticipantLineMotionRender : MonoBehaviour
{
    //private LineRenderer lineRenderer;

    //get points from the participant in the CSV
    private Transform[] points;

    public string csvFileName = "motion_data";


    //class to represent a tracked object
    //this allows you to create multiple of them
    [System.Serializable]
    public class TrackedObject
    {

        public string name;
        public LineRenderer lineRenderer;
        public int posX;
        public int posY;
        public int posZ;

    }

    //list of all tracked objects in the inspector
    public List<TrackedObject> trackedObjects;

    TextAsset csv;

    private void Awake()
    {
        
        LoadLineRenderer();
    }


    public void LoadLineRenderer()
    {
        //Loadd the file from Resources
        csv = Resources.Load<TextAsset>(csvFileName);

        //check if file exists
        if (csv == null)
        {
            Debug.Log("FILE NOT FOUND");
            return;
        }

        //split the entire .csv into individual lines
        string[] lines = csv.text.Split('\n');
        bool skipHeader = true;

        //create a dictionary of tracked objects
        //it maps tracked object to its list of position points
        //Example: LeftHand -> [(x1,y1,z1), (x2,y2,z2)]
        Dictionary<TrackedObject, List<Vector3>> objectPoints = new Dictionary<TrackedObject, List<Vector3>>();


        //Initialize empty lists for each object
        foreach (var obj in trackedObjects)
        {
            objectPoints[obj] = new List<Vector3>();
        }

        //Loop through every row in the csv
        foreach (string raw in lines)
        {
            //ignore empty lines
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            if (skipHeader)
            {
                skipHeader = false;
                continue;
            }

            // Split the row into columns
            string[] cols = raw.Split(',');

            // Loop through each tracked object (LeftHand, RightHand, etc.)
            foreach (var obj in trackedObjects)
            {
                // Make sure the row actually has enough columns
                if (cols.Length <= obj.posZ) continue;


                //Parse the x,y.z values for each object
                float x = float.Parse(cols[obj.posX]);
                float y = float.Parse(cols[obj.posY]);
                float z = float.Parse(cols[obj.posZ]);

                //store the point
                objectPoints[obj].Add(new Vector3(x, y, z));
            }
        }

        foreach (var obj in trackedObjects)
        {
            // Set number of points in the LineRenderer
            obj.lineRenderer.positionCount = objectPoints[obj].Count;
            
            //Apply the points
            obj.lineRenderer.SetPositions(objectPoints[obj].ToArray());
        }
    }
}
