using UnityEngine;

public class SmoothFollowWithCurrentOffset : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
