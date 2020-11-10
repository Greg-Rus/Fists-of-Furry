using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    public Vector3 OffsetFromTarget;
    public float PositionDamping;

    public Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Target == null) return;
        
        var desiredPosition = Target.position + OffsetFromTarget;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, PositionDamping * Time.smoothDeltaTime);
    }
}
