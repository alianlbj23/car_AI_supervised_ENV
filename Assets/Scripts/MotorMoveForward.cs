using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMoveForward : MonoBehaviour
{
    ArticulationBody bd;
    float stiffness = 0f;
    float damping = 1000f;
    float forceLimit = 100f;
    float voltage = 0;

    void Awake()
    {
        bd = GetComponent<ArticulationBody>();
    }

    public void SetVoltage(float newvoltage)
    {
        voltage = newvoltage;
    }

    void Start()
    {
        ArticulationDrive currentDrive = bd.xDrive;
        currentDrive.stiffness = stiffness;
        currentDrive.damping = damping;
        currentDrive.forceLimit = forceLimit;
        bd.xDrive = currentDrive;
    }

    // Update is called once per frame
    void Update()
    {
        setSpeed(voltage);
    }

    void setSpeed(float voltage_wheel)
    {
        var drive = bd.xDrive;
        drive.targetVelocity = voltage_wheel;
        bd.xDrive = drive;
    }
}
