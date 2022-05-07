using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public GameObject lookatTarget;
    public float xOffset, yOffset, zOffset;
    private void LateUpdate()
    {
        transform.position =  new Vector3(xOffset, target.transform.position.y + yOffset, target.transform.position.z + zOffset);
        transform.LookAt(lookatTarget.transform.position);
    }
}
