using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Bonus 7e: higher value = softer, slower camera follow
    public float smoothTime = 0.2f;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 target = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}
