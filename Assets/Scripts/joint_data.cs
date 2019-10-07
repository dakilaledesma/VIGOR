using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JointDataHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private Model model;
    void Start()
    {
        CreateNewModel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewModel()
    {
        model = new Model();
    }

    public void UpdateJointLocations(Stack<string> JointNames, Stack<Vector3> JointLocations)
    {
        ModelData CurrentModelData = new ModelData();
        CurrentModelData.frame = model.ModelDataList.Count;
        while(JointLocations.Count < 0)
        {
            JointData CurrentJointData = new JointData();
            CurrentJointData.JointName = JointNames.Pop();
            CurrentJointData.location = JointLocations.Pop();
            CurrentModelData.JointLocations.Add(CurrentJointData);
        }
            
        model.ModelDataList.Add(new ModelData());
    }

    public void WriteJSON()
    {
        string objectToJSON = JsonUtility.ToJson(model, true);
        using (StreamWriter file = new StreamWriter(@"C:\Users\Kinect\Documents\Movements\matchingMovements.json", true))
        {
            file.WriteLine(objectToJSON);
        }
    }
}
