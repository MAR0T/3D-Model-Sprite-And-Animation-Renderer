using UnityEngine;

public class MouseCameraMovement : MonoBehaviour
{
    private Camera cam;

    private float currentCameraSize;

    public float ZoomSpeed = 5f;

    public float MinCameraSize = 6, MaxCameraSize = 18;

    private bool dragging;

    private Vector3 dragStartPos;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        currentCameraSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseDrag();
        HandleZoonOnMousePosition();
    }

    private void HandleZoonOnMousePosition()
    {
        Vector3 oldPos = GetCurrentMouseWorldPoint();
        currentCameraSize = Mathf.Clamp(currentCameraSize - Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed, MinCameraSize, MaxCameraSize);
        cam.orthographicSize = currentCameraSize;
        Vector3 newPos = GetCurrentMouseWorldPoint();
        transform.position -= newPos - oldPos;
    }

    void HandleMouseDrag()
    {
        if (!dragging && Input.GetMouseButtonDown(1))
        {
            dragStartPos = GetCurrentMouseWorldPoint();
            dragging = true;
        }
        if (dragging && Input.GetMouseButton(1))
        {
            Vector3 actualPos = GetCurrentMouseWorldPoint();
            Vector3 dragDelta = actualPos - dragStartPos;

            if (Mathf.Abs(dragDelta.x) > 0.00001f || Mathf.Abs(dragDelta.y) > 0.00001f)
            {
                transform.Translate(-dragDelta);
            }
        }
        if (dragging && Input.GetMouseButtonUp(1))
        {
            dragging = false;
        }
    }

    public static Vector3 GetCurrentMouseWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    public void CenterOnCore()
    {
        transform.position = new Vector3(0, 0, -10);
    }
}