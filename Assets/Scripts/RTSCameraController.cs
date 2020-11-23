using UnityEngine;

public class RTSCameraController : MonoBehaviour {

    [Header("Pan")]
    [SerializeField] private float panSpeed = 20f;
    
    [Header("Zoom")]
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float minHeight = 3f;
    [SerializeField] private float maxHeight = 50f;

    [Header("Rotation")]
    [SerializeField] private float rotSpeed = 2f;

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
        camYPOS = Mathf.Clamp(camYPOS - zoom * verticalSpeed, minHeight, maxHeight);
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, camYPOS, cam.transform.localPosition.z);
        cam.transform.LookAt(transform);

        // Camera Rotation
        if(rightClicked) {
            transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X") * rotSpeed));
        }

    }
}
