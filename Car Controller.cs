using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollliders collliders;               //Colliders




    public MeshRenderer FlMesh;                      
    public MeshRenderer FrMesh;                        //Meshes
    public MeshRenderer RlMesh;
    public MeshRenderer RrMesh;


    public float gasInput;
    public float steeringInput;                         //INPUTS
    public float speed;
    private Rigidbody playerRb;
    public AnimationCurve steeringCurve;
    


    public float motorPower = 500f;                       //power
    public float brakePower;
    public float slipAngle;
    public float brakeInput;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        speed = playerRb.velocity.magnitude;
        ApplyWheelPosition();
        CheckInput();
        Motor();
        ApplySteering();
        ApplyBrake();
    }

    void ApplyWheelPosition()
    {
        UpdateWheel(collliders.FlWheel, FlMesh);
        UpdateWheel(collliders.RlWheel, RlMesh);    
        UpdateWheel(collliders.FrWheel,FrMesh);
        UpdateWheel(collliders.RrWheel, RrMesh);
            

        
    }
    void UpdateWheel(WheelCollider colls,MeshRenderer  wheelmesh)
    {
        Quaternion quat;
        Vector3 position;
        colls.GetWorldPose(out position, out quat);
        wheelmesh.transform.position = position;
        wheelmesh.transform.rotation = quat;
    }



    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward,playerRb.velocity-transform.forward);
        if (slipAngle < 120f)
        {
            if (gasInput < 0)
            {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            else
            {
                brakeInput = 0;
            }
        }
        else
        {
            brakeInput = 0;
        }
    }


    void Motor()
    {
        collliders.RrWheel.motorTorque = motorPower*gasInput;
        collliders.RlWheel.motorTorque = motorPower*gasInput;
    }
      

    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
      //  steeringAngle += Vector3.SignedAngle(transform.forward, playerRb.velocity + transform.forward, Vector3.up);
      //  steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        collliders.FrWheel.steerAngle = steeringAngle;
        collliders.FlWheel.steerAngle = steeringAngle; 
    }
    void ApplyBrake()
    {
        collliders.FrWheel.brakeTorque = brakePower*brakeInput*0.7f;
        collliders.FlWheel.brakeTorque = brakePower * brakeInput * 0.7f;
        collliders.RrWheel.brakeTorque = brakePower * brakeInput * 0.3f;
        collliders.RlWheel.brakeTorque = brakePower * brakeInput * 0.3f;
    }


    [System.Serializable]
    public class WheelCollliders
    {
        public WheelCollider FlWheel;
        public WheelCollider FrWheel;
        public WheelCollider RlWheel;
        public WheelCollider RrWheel;
    }


    

   
   
       
    
}
