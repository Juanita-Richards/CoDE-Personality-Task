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

    [System.Serializable]
    public class TrackedObject
    {

        public string name;
        public LineRenderer lineRenderer;
        public int posX;
        public int posY;
        public int posZ;

    }

    public List<TrackedObject> trackedObjects;

    TextAsset csv;

    private void Awake()
    {
        
        LoadLineRenderer();
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    private void Update()
    {
        
    }

    public void LoadLineRenderer()
    {

        csv = Resources.Load<TextAsset>(csvFileName);

        if (csv == null)
        {
            Debug.Log("FILE NOT FOUND");
            return;
        }

        string[] lines = csv.text.Split('\n');
        bool skipHeader = true;

        Dictionary<TrackedObject, List<Vector3>> objectPoints = new Dictionary<TrackedObject, List<Vector3>>();

        foreach (var obj in trackedObjects)
        {
            objectPoints[obj] = new List<Vector3>();
        }

        foreach (string raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            if (skipHeader)
            {
                skipHeader = false;
                continue;
            }

            string[] cols = raw.Split(',');

            foreach (var obj in trackedObjects)
            {
                if (cols.Length <= obj.posZ) continue;

                float x = float.Parse(cols[obj.posX]);
                float y = float.Parse(cols[obj.posY]);
                float z = float.Parse(cols[obj.posZ]);


                objectPoints[obj].Add(new Vector3(x, y, z));
            }
        }

        foreach (var obj in trackedObjects)
        {
            obj.lineRenderer.positionCount = objectPoints[obj].Count;
            obj.lineRenderer.SetPositions(objectPoints[obj].ToArray());
        }
    }
}
