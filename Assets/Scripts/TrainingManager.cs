using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using Math = System.Math;
using System.Reflection;
using WebSocketSharp;
using MiniJSON;




public class TrainingManager : MonoBehaviour
{
    string topicName = "/Unity_2_AI";
    string topicName2 = "/Unity_2_AI_stop_flag";
    string topicName_receive = "/AI_2_Unity";
    private WebSocket socket;
    private string rosbridgeServerUrl = "ws://localhost:9090";
    public Robot robot;
    [SerializeField]
    GameObject anchor1, anchor2, anchor3, anchor4;
    [SerializeField]
    GameObject target;
    enum Phase
    {
        Freeze,
        Run
    }
    Phase phase;
    Vector3 newTarget;
    List<float> wheelvelocity = new List<float>();
    public System.Random random = new System.Random();
    Transform base_footprint;
    [System.Serializable]
    public class RobotNewsMessage
    {
        public string op;
        public string topic;
        public MessageData msg;
    }
    [System.Serializable]
    public class MessageData
    {
        public LayoutData layout;
        public float[] data;
    }
    [System.Serializable]
    public class LayoutData
    {
        public int[] dim;
        public int data_offset;
    }
    void Awake()
    {
        base_footprint = robot.transform.Find("base_link");
    }

    Transform baselink;
    Vector3 carPos;
    string mode = "train";
    float key = 0;
    public float delayInSeconds = 0f;
    void Start()
    {
        StartCoroutine(DelayedExecution());
    }

    IEnumerator DelayedExecution()
    {
        delayInSeconds = 0.001f;
        yield return new WaitForSeconds(delayInSeconds);
        baselink = robot.transform.Find("base_link");
        newTarget = GetTargetPosition(target, newTarget);
        socket = new WebSocket(rosbridgeServerUrl);

        if (mode == "train")
        {
            socket.OnOpen += (sender, e) =>
            {
                SubscribeToTopic(topicName_receive);
            };
            socket.OnMessage += OnWebSocketMessage;
        }


        socket.Connect();

        State state = robot.GetState(newTarget);

        Send(state);
        // 在延迟后执行的代码

        Debug.Log("程式碼在 " + delayInSeconds + " 秒後執行了！");
    }



    void Update()
    {
        if (mode == "train")
        {
            if (key == 1)
            {
                State state = robot.GetState(newTarget);
                Debug.Log("hello");
                Send(state);
                key = 0;
            }
        }
        else
        {
            CarMove();
        }
    }

    Vector3 GetTargetPosition(GameObject obj, Vector3 pos)
    {
        Transform objTransform = obj.transform;
        pos = objTransform.position;
        return pos;
    }

    void CarMove()
    {

        if (Input.GetKey(KeyCode.W))
        {
            WheelSpeed(600f, 600f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            WheelSpeed(600f, -600f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            WheelSpeed(-600f, 600f);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            exitUnityAndStoreData();
        }
        else
        {
            WheelSpeed(0f, 0f);
        }


    }

    void exitUnityAndStoreData()
    {
        Dictionary<string, object> message1 = new Dictionary<string, object>
        {
            { "op", "publish" },
            { "id", "1" },
            { "topic", topicName2 },
            { "msg", new Dictionary<string, string>
                {
                    { "data", "0"}
                }
            }
        };
        string jsonMessage1 = MiniJSON.Json.Serialize(message1);
        try
        {
            socket.Send(jsonMessage1);
        }
        catch
        {
            Debug.Log("error-send");
        }
        UnityEditor.EditorApplication.isPlaying = false;
    }

    void WheelSpeed(float leftWheel, float rightWheel)
    {
        Robot.Action action = new Robot.Action();
        action.voltage = new List<float>();

        action.voltage.Add(leftWheel);
        action.voltage.Add(rightWheel);
        robot.DoAction(action);

        if (leftWheel != 0f && rightWheel != 0f)
        {
            State state = robot.GetState(newTarget);
            Send(state);
        }
    }



    private void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        string jsonString = e.Data;
        RobotNewsMessage message = JsonUtility.FromJson<RobotNewsMessage>(jsonString);
        float[] data = message.msg.data;
        switch (data[0])
        {
            case 0:
                Robot.Action action = new Robot.Action();
                action.voltage = new List<float>();
                data[1] = (float)data[1];
                float left = 0f;
                float right = 0f;
                Debug.Log(data[1]);
                if(data[1] == 0f){
                    left = 300f;
                    right = 300f;
                }


                action.voltage.Add(left);
                action.voltage.Add(right);
                robot.DoAction(action);

                key = 1;

                break;

        }
    }

    State updateState(Vector3 newTarget, List<float> wheelvelocity)
    {
        State state = robot.GetState(newTarget);

        return state;
    }





    void Send(object data)
    {
        var properties = typeof(State).GetProperties();
        Dictionary<string, object> stateDict = new Dictionary<string, object>();

        foreach (var property in properties)
        {
            string propertyName = property.Name;
            var value = property.GetValue(data);
            stateDict[propertyName] = value;
        }

        string dictData = MiniJSON.Json.Serialize(stateDict);

        Dictionary<string, object> message = new Dictionary<string, object>
        {
            { "op", "publish" },
            { "id", "1" },
            { "topic", topicName },
            { "msg", new Dictionary<string, object>
                {
                    { "data", dictData}
                }
           }
        };

        string jsonMessage = MiniJSON.Json.Serialize(message);

        try
        {
            socket.Send(jsonMessage);
        }
        catch
        {
            Debug.Log("error-send");
        }
    }
    private void SubscribeToTopic(string topic)
    {
        string subscribeMessage = "{\"op\":\"subscribe\",\"id\":\"1\",\"topic\":\"" + topic + "\",\"type\":\"std_msgs/msg/Float32MultiArray\"}";
        socket.Send(subscribeMessage);
    }
}

