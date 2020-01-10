using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.Sensor.BodyTracking;
using System.IO;

public class DebugRenderer : MonoBehaviour
{
    Device device;
    BodyTracker tracker;
    Skeleton skeleton;
    GameObject[] debugObjects;
    public Renderer renderer;
    List<string> csv_frames;

    private void OnEnable()
    {
        csv_frames = new List<string>();
        this.device = Device.Open(0);
        var config = new DeviceConfiguration
        {
            ColorResolution = ColorResolution.r720p,
            ColorFormat = ImageFormat.ColorBGRA32,
            DepthMode = DepthMode.NFOV_Unbinned
        };
        device.StartCameras(config);

        var calibration = device.GetCalibration(config.DepthMode, config.ColorResolution);
        this.tracker = BodyTracker.Create(calibration);
        debugObjects = new GameObject[(int)JointId.Count];
        for (var i = 0; i < (int)JointId.Count; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = Enum.GetName(typeof(JointId), i);
            cube.transform.localScale = Vector3.one * 0.4f;
            debugObjects[i] = cube;
        }
    }

    private void OnDisable()
    {
        string csv = "";
        for (int i = 0; i < csv_frames.Count - 1; i++)
        {
            csv += csv_frames[i] + "\n";
        }

        string file = "Assets/Resources/akrecord-" + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + ".csv";
        
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(csv);
        writer.Close();

        if (tracker != null)
        {
            tracker.Dispose();
        }
        if (device != null)
        {
            device.Dispose();
        }
    }

    void Update()
    {
        using (Capture capture = device.GetCapture())
        {
            tracker.EnqueueCapture(capture);
            var color = capture.Color;
            if (color.WidthPixels > 0)
            {
                Texture2D tex = new Texture2D(color.WidthPixels, color.HeightPixels, TextureFormat.BGRA32, false);
                tex.LoadRawTextureData(color.GetBufferCopy());
                tex.Apply();
                renderer.material.mainTexture = tex;
            }
        }
        
        using (var frame = tracker.PopResult())
        {
            Debug.LogFormat("{0} bodies found.", frame.NumBodies);
            if (frame.NumBodies == 1)
            {
                var bodyId = frame.GetBodyId(0);
                Debug.LogFormat("bodyId={0}", bodyId);
                this.skeleton = frame.GetSkeleton(0);
                string csv_joints = "";
                for (var i = 0; i < (int)JointId.Count; i++)
                {
                    var joint = this.skeleton.Joints[i];
                    var pos = joint.Position;
                    var rot = joint.Orientation;
                    var v = new Vector3(pos[0], -pos[1], pos[2]) * 0.004f;
                    Vector3 vect = new Vector3(pos[0], -pos[1], pos[2]) * 0.004f;
                    csv_joints += vect.x + "," + vect.y + "," + vect.z + ",";
                    var r = new Quaternion(rot[1], rot[2], rot[3], rot[0]);
                    var obj = debugObjects[i];
                    obj.transform.SetPositionAndRotation(v, r);
                }
                csv_frames.Add(csv_joints);

            }
        }
    }
}
