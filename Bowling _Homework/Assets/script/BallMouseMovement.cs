using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BallMouseMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private float fixedY;
    private float fixedZ;

    public RectTransform powerUI;
    public float maxForce = 100f;
    
    private Rigidbody rb;
    private Camera mainCamera;

    private bool isAiming = false;
    private bool isLaunched = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
        
    }

    void Update()
    {
        if (isLaunched) return;
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isAiming = true;
        }
        
        if (Mouse.current.leftButton.wasReleasedThisFrame && isAiming)
        {
            LaunchBall();
        }
        
        if (isAiming && Mouse.current.leftButton.isPressed)
        {
            MoveBall();
        }
       
    }

    void MoveBall()
    {
        float mouseX = Mouse.current.position.ReadValue().x;
        
        float t = Mathf.Clamp01(mouseX / Screen.width);
        
        float targetX = Mathf.Lerp(pointA.position.x, pointB.position.x, t);
        
        transform.position = new Vector3(targetX, fixedY, fixedZ);
    }
    
    void LaunchBall()
    {
        Vector3 direction = Vector3.forward;

        float currentY = powerUI.anchoredPosition.y;
        float t = Mathf.InverseLerp(-1200f, -245f, currentY);
        float currentForce = Mathf.Lerp(0f, maxForce, t);

        isAiming = false;
        isLaunched = true;

        rb.AddForce(direction * currentForce, ForceMode.Impulse);
        
        StartCoroutine(ResetGame());
    }
    
    Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mouseScreen);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        
        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position;
    }
    
    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
