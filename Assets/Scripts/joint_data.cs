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
        model.ModelDataList = new List<ModelData>();
    }

    public void UpdateJointLocations(Stack<string> JointNames, Stack<Vector3> JointLocations)
    {
        ModelData CurrentModelData = new ModelData();
        CurrentModelData.JointLocations = new List<JointData>();
        CurrentModelData.frame = model.ModelDataList.Count;
        while(JointLocations.Count > 0)
        {
            JointData CurrentJointData = new JointData();
            CurrentJointData.JointName = JointNames.Pop();
            CurrentJointData.location = JointLocations.Pop();
            CurrentModelData.JointLocations.Add(CurrentJointData);
        }
            
        model.ModelDataList.Add(CurrentModelData);
    }

    public string StreamJointLocations(Stack<string> JointNames, Stack<Vector3> JointLocations)
    {
        CreateNewModel();
        ModelData CurrentModelData = new ModelData();
        CurrentModelData.JointLocations = new List<JointData>();
        CurrentModelData.frame = model.ModelDataList.Count;
        while (JointLocations.Count > 0)
        {
            JointData CurrentJointData = new JointData();
            CurrentJointData.JointName = JointNames.Pop();
            CurrentJointData.location = JointLocations.Pop();
            CurrentModelData.JointLocations.Add(CurrentJointData);
        }

        model.ModelDataList.Add(CurrentModelData);

        string objectToJSON = JsonUtility.ToJson(model, true);
        return objectToJSON;
    }


    public void WriteJSON()
    {
        string objectToJSON = JsonUtility.ToJson(model, true);
        string JSONPath = Application.dataPath + "/JSONs/";
        DirectoryInfo JSONDirectory = new DirectoryInfo(JSONPath);

        using (StreamWriter file = new StreamWriter("Assets/JSONs/" + JSONDirectory.GetFiles("*.json").Length.ToString() + ".json", true))
        {
            file.WriteLine(objectToJSON);
        }
    }

    public int GetDataCount()
    {
        return model.ModelDataList.Count;
    }
}
