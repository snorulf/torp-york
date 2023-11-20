using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField]
    float distanceAway = 5;
    [SerializeField]
    float distanceUp = 2;
    [SerializeField]
    float smooth = 3;
    [SerializeField]
    Transform followXForm;
    [SerializeField]
    Vector3 offset = new Vector3(0f, 1.5f, 0f);
    [SerializeField]
    Vector3 lookDir;
    [SerializeField]
    Vector3 lookOffset = new Vector3(0f, 1f, 0f);
    [SerializeField]
    Vector3 targetPosition;

    private float camSmoothDampTime = 0.1f;
    [SerializeField]
    Vector3 velocityCamSmooth = Vector3.zero;

    void Start()
    {
        followXForm = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 characterOffset = followXForm.position + offset;

        lookDir = characterOffset - this.transform.position;
        lookDir.y = 0;
        lookDir.Normalize();
        //Debug.DrawRay(this.transform.position, lookDir, Color.green);

        targetPosition = followXForm.position + followXForm.up * distanceUp - lookDir * distanceAway;

        // Debug.DrawRay(followXForm.position, Vector3.up * distanceUp, Color.red);
        // Debug.DrawRay(followXForm.position, -1f * followXForm.forward * distanceAway, Color.blue);
        // Debug.DrawLine(followXForm.position, targetPosition, Color.magenta);

        smoothPosition(transform.position, targetPosition);

        transform.LookAt(followXForm);
    }

    private void smoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }
}
