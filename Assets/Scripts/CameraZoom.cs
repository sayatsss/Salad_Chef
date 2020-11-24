using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public float minimum_Zoom = 6;
    public float maximum_Zoom = 10;


    private float distance;
    private float val;
    private Camera scneCamera;

    private void Start()
    {
        scneCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        val = Mathf.Clamp(distance, minimum_Zoom, maximum_Zoom);
        scneCamera.orthographicSize = Mathf.Lerp(scneCamera.orthographicSize, val, Time.deltaTime);
    }
}
