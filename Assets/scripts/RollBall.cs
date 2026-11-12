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
    
    private float clickForce;
    
    private float pressStartTime;
    private float holdDuration;

    public float FPerSec = 20f;
    private float FMaxTime = 3f;
    public Vector3 forceDirection;

    private LineRenderer aimLine;
    public Material aimMaterial;

    private Vector3 SpawnPosition;
    public GameObject ballPrefab;
    
    private GameManager  gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        SpawnPosition = transform.position;

        aimLine = GetComponent<LineRenderer>();
        if (aimLine == null)
        {
            aimLine = gameObject.AddComponent<LineRenderer>();
        }
            
            aimLine.sharedMaterial = aimMaterial;
            aimLine.positionCount = 2;
            aimLine.startWidth = 0.05f;
            aimLine.endWidth = 0.05f;
            aimLine.enabled = false;
        

        gameManager = FindObjectOfType<GameManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            holdingButt = true;
            pressStartTime = Time.time;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && holdingButt)
        {
            holdingButt = false;
            aimLine.enabled = false;

            holdDuration = Mathf.Clamp(
                Time.time - pressStartTime, 0f, FMaxTime);

            clickForce = holdDuration * FPerSec;

            Ray mouseCameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit mouseRaycastHit;

            if (Physics.Raycast(mouseCameraRay, out mouseRaycastHit)
                && mouseRaycastHit.collider.gameObject == gameObject)
            {
                forceDirection = (transform.position - mouseRaycastHit.point).normalized;

                rb.AddForceAtPosition(
                    forceDirection * clickForce,
                    mouseRaycastHit.point,
                    ForceMode.Impulse);
            }
        }

        clickForce = holdDuration * FPerSec;

        if (holdingButt) //check if left mouse button was just clicked
        {
            //where did the mouse click in terms of the world. 
            Ray mouseCameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit mouseRaycastHit;

            if(Physics.Raycast(mouseCameraRay, out mouseRaycastHit)){
                
                //did the mouse raycast just hit this ball's collider?
                if(mouseRaycastHit.collider.gameObject == this.gameObject)
                {
                    //push the ball away from the position where the raycast hit it
                    forceDirection = (transform.position - mouseRaycastHit.point).normalized;
                }
            }
            holdDuration = Mathf.Clamp(Time.time - pressStartTime, 0f, FMaxTime);

            Vector3 start = transform.position + Vector3.up * 0.7f;
            float length = Mathf.Max(0, holdDuration * 2f);
            aimLine.enabled = true;
            aimLine.SetPosition(0, start);
            aimLine.SetPosition(1, start + forceDirection * length);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Instantiate(ballPrefab, SpawnPosition, Quaternion.identity);

            Destroy(gameObject);
        }

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin"))
        {
            gameManager.HitPin(collision.gameObject);
        }
    }


        
 
}
