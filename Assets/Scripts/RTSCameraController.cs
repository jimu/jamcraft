using UnityEngine;

public class RTSCameraController : MonoBehaviour {

    [Header("Pan")]
    [SerializeField] private float panSpeed = 20f;
    
    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float maxFOV = 90f;

    [Header("Rotation")]
    [SerializeField] private float rotSpeed = 2f;

    [Header("Camera Height")]
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float minHeight = 3f;
    [SerializeField] private float maxHeight = 10f;

    // References
    private Camera cam;

    // Other
    float camYPOS;

    void Start() {
        cam = GetComponentInChildren<Camera>();
        cam.transform.LookAt(transform);
        camYPOS = cam.transform.position.y;
    }

    void Update() {

        // X Z Camera Movement
        Vector3 movement = Vector3.zero;
        bool rightClicked = Input.GetMouseButton(1);
        bool middleClicked = Input.GetMouseButton(2);

        if(Input.GetKey("w")) {
            movement -= transform.right;
        }
        if(Input.GetKey("s")) {
            movement += transform.right;
        }
        if(Input.GetKey("a")) {
            movement -= transform.forward;
        }
        if(Input.GetKey("d")) {
            movement += transform.forward;
        }

        movement.Normalize();
        transform.position = transform.position + (movement * panSpeed * Time.deltaTime);

        // Zoom Control
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        cam.fieldOfView -= zoom * zoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFOV, maxFOV);

        // Camera Rotation
        if(rightClicked) {
            transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X") * rotSpeed));
        }

        // Camera Height
        if(middleClicked) {
            camYPOS = Mathf.Clamp(camYPOS + Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime, minHeight, maxHeight);
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, camYPOS, cam.transform.localPosition.z);
            cam.transform.LookAt(transform);
        }

    }
}
