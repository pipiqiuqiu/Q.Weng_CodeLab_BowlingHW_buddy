using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollBall : MonoBehaviour

{
    
    Rigidbody rb;
    //public float Force=100f;
    //InputAction bowlbutt;

    private bool holdingButt;
    private bool roll;

    public LayerMask personaLayer;
    private float clickForce;
    
    private float pressStartTime;
    private float holdDuration;

    public float FPerSec = 20f;
    private float FMaxTime = 3f;
    public Vector3 forceDirection;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * Force);
        //bowlbutt = InputSystem.actions.FindAction("Bowl");
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            holdingButt = true;
            pressStartTime = Time.time;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            holdingButt = false;
            holdDuration = Mathf.Clamp(Time.time - pressStartTime, 0f, FMaxTime);
            Debug.Log(holdDuration);
        }

        clickForce = holdDuration * FPerSec;

        if (Mouse.current.leftButton.wasReleasedThisFrame) //check if left mouse button was just clicked
        {
            //where did the mouse click in terms of the world. 
            Ray mouseCameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit mouseRaycastHit;

            if(Physics.Raycast(mouseCameraRay, out mouseRaycastHit)){
                
                //did the mouse raycast just hit this ball's collider?
                if(mouseRaycastHit.collider.gameObject == this.gameObject)
                {
                    //push the ball away from the position where the raycast hit it
                    Vector3 forceDirection = (transform.position - mouseRaycastHit.point).normalized;
                   
                    rb.AddForceAtPosition(forceDirection * clickForce, 
                        mouseRaycastHit.point,ForceMode.Impulse);
                }
            }
            
        }

        //if (holdingButt && rb.linearVelocity == Vector3.zero)
        {
            //find the direction the mouse is pointing towards   
          //  Debug.DrawRay(transform.position, forceDirection * holdDuration, Color.red);
        }
    }

        
        /*
    void FixedUpdate()
    {
        if (holdingButt && rb.linearVelocity == Vector3.zero)
        {
         //find the direction the mouse is pointing towards
         Debug.DrawRay(transform.position, forceDirection * holdDuration, Color.red);
        }


    }

    void StartRoll()
    {
        if (Physics.Raycast(transform.position, FindDirection(), 1000f,personaLayer) == false)
        {
            rb.isKinematic = false;
            rb.AddForce(Force * transform.TransformDirection(FindDirection()), ForceMode.Impulse);
            roll = false;
        }
        Vector3 pointTo = FindDirection();
    }

    Vector3 FindDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f;
        Vector3 dir = Camera.main.ScreenToWorldPoint(mousePosition);
        dir.z = 10f;
        return dir.normalized;
    }*/
}
