using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Range(0.001f, 0.5f)]
    public float sensetivity = 0.055f;
    private Vector2 start;
    // Use this for initialization
    void Start()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        start = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        var size = Camera.main.orthographicSize;
        size -= Input.mouseScrollDelta.y;
        size = Mathf.Clamp(size, 1, 1000);
        Camera.main.orthographicSize = size;
        var x_y = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        if (Input.GetMouseButtonDown(2))
        {
            start = Input.mousePosition;
        }
        if(Input.GetMouseButton(2))
        {
            x_y += (start - (Vector2)Input.mousePosition) * sensetivity;
            start = Input.mousePosition;
        }
        Camera.main.transform.position = new Vector3(x_y.x, x_y.y, Camera.main.transform.position.z);
    }
}
