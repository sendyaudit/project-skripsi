using UnityEngine;
using Vuforia;

public class ARManager : MonoBehaviour
{
    public float zoomSpeed = 0.001f;
    public float minScale = 0.3f;
    public float maxScale = 1.5f;
    public float smoothSpeed = 5f;

    public float rotationSpeed = 0.3f; // kecepatan rotasi (lebih besar biar responsif)

    public Transform[] targets;
    private Transform activeTarget;

    void Update()
    {
        UpdateActiveTarget();

        if (activeTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, activeTarget.position, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, activeTarget.rotation, Time.deltaTime * smoothSpeed);

            HandleZoomGesture();
            HandleRotationGesture();
        }
    }

    void UpdateActiveTarget()
    {
        foreach (Transform target in targets)
        {
            var observer = target.GetComponent<ObserverBehaviour>();
            if (observer != null && observer.TargetStatus.Status == Status.TRACKED)
            {
                if (activeTarget != target)
                {
                    activeTarget = target;
                }
                return;
            }
        }
        activeTarget = null;
    }

    void HandleZoomGesture()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Transform zoomTarget = activeTarget.childCount > 0 ? activeTarget.GetChild(0) : null;

            if (zoomTarget != null)
            {
                Vector3 newScale = zoomTarget.localScale + Vector3.one * difference * zoomSpeed;
                newScale = ClampVector3(newScale, minScale, maxScale);
                zoomTarget.localScale = Vector3.Lerp(zoomTarget.localScale, newScale, Time.deltaTime * 10f);
            }
        }
    }

    void HandleRotationGesture()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Transform rotateTarget = activeTarget.childCount > 0 ? activeTarget.GetChild(0) : null;

                if (rotateTarget != null)
                {
                    float rotateX = touch.deltaPosition.y * rotationSpeed; // gerakan vertikal
                    float rotateY = -touch.deltaPosition.x * rotationSpeed; // gerakan horizontal (dibalik biar natural)

                    // rotasi berdasarkan gerakan jari
                    rotateTarget.Rotate(Vector3.up, rotateY, Space.World);
                    rotateTarget.Rotate(Vector3.right, rotateX, Space.World);
                }
            }
        }
    }

    Vector3 ClampVector3(Vector3 value, float min, float max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min, max),
            Mathf.Clamp(value.y, min, max),
            Mathf.Clamp(value.z, min, max)
        );
    }
}
