using UnityEngine;
using UnityEngine.InputSystem;

public class RollBall : MonoBehaviour
{
    private Rigidbody rb;
    InputAction bowlButt;

    private bool holdingButt;
    private bool roll;

    public float rollPower;
    public LayerMask personLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * 1000f);
        bowlButt = InputSystem.actions.FindAction("Bowl");
    }
    
    void Update()
    {
        if (bowlButt.WasPressedThisFrame())
        {
            holdingButt = true;
            //rb.AddForce(transform.forward * 1000f);
            //startRoll();
        }else if (bowlButt.WasReleasedThisFrame())
        {
            holdingButt = false;
            roll = true;
        }
    }

    void FixedUpdate()
    {
        if (holdingButt && rb.linearVelocity == Vector3.zero)
        {
            Debug.DrawRay(transform.position, FindDirection() * 1000f, Color.red);
        }

        if (roll)
        {
            startRoll();
        }
    }
    
    void startRoll()
    {
        if (Physics.Raycast(transform.position, FindDirection(), 1000f, personLayer) == false)
        {
            rb.isKinematic = false;
            rb.AddForce(rollPower * transform.TransformDirection(FindDirection()), ForceMode.Impulse);
            //Vector3 pointTo = FindDirection();
            roll = false;
        }
        
        
    }

    Vector3 FindDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f;
        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePosition);
        direction.z = 10f;
        return direction.normalized;
    }
}
