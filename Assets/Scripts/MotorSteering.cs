using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSteering : MonoBehaviour
{
    ArticulationBody bd;
    const float RAD2DEG = 57.295779513f;
    // float smooth = 1.0f;
    float speed = 500;
    float stiffness = 1000f;
    float damping = 0f;
    float forceLimit = 100;

    void Awake()
    {
        bd = GetComponent<ArticulationBody>();
        

    }

    public void SetAngle(float targetRadian)
    {   
        ArticulationDrive currentDrive = bd.xDrive;

        float targetAngle = targetRadian * RAD2DEG;
        // print("target " + targetAngle);
        // targetAngle = (targetAngle + 360) % 360;
        
        // float currAngle = transform.rotation.eulerAngles[1];
        // if (currAngle > 358 || currAngle < 2) currAngle = 0;

        float newTargetDelta; 

        if(currentDrive.target < targetAngle) { //positive clockwise //currAngle
            newTargetDelta = 1 * Time.fixedDeltaTime * speed;
        } else {
            newTargetDelta = -1 * Time.fixedDeltaTime * speed;
        }
        // Debug.Log("target angle: " + targetAngle);
        // Debug.Log("curr angle: " + currAngle);
        // Debug.Log("curr target: " + currentDrive.target);
        // Debug.Log("tar delta: " + newTargetDelta);

        // Debug.Log(currentDrive.upperLimit);
        // Debug.Log(currentDrive.lowerLimit);
   
        if (newTargetDelta + currentDrive.target > currentDrive.upperLimit)
        {
            currentDrive.target = currentDrive.upperLimit;
        }
        else if (newTargetDelta + currentDrive.target < currentDrive.lowerLimit)
        {
            currentDrive.target = currentDrive.lowerLimit;
        }
        else
        {
            currentDrive.target += newTargetDelta;
        }

        // currentDrive.target += newTargetDelta;
        
    

        // Debug.Log("later target: " + currentDrive.target);
                
        bd.xDrive = currentDrive;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ArticulationDrive currentDrive = bd.xDrive;
        currentDrive.stiffness = stiffness;
        currentDrive.damping = damping;
        currentDrive.forceLimit = forceLimit;
        bd.xDrive = currentDrive;
    }
}
