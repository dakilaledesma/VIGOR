using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointData
{
    public string JointName;
    public Vector3 location;
}

[System.Serializable]
public class ModelData
{
    public int frame;
    public List<JointData> JointLocations;
}


[System.Serializable]
public class Model
{
    public List<ModelData> ModelDataList;
}