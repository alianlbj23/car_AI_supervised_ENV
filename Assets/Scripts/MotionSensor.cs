using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MotionSensor : MonoBehaviour
{   
    const float DEG2RAD = 0.01745329251f;
    ArticulationBody bd;

    public Vector3 x { get { return transform.position; } } //local position
    public Vector3 x_world { get { return transform.position; } }
    public Vector3 theta { get { return transform.rotation.eulerAngles * DEG2RAD; } } //rotation angles
    public Vector3 v { get { return bd.velocity; } }
    public Vector3 AngularV { get { return bd.transform.InverseTransformDirection(bd.angularVelocity); } } //radian
    public Quaternion q { get { return transform.rotation; } } //rotation angles

    void Awake()
    {
        bd = GetComponent<ArticulationBody>();
    }
}
