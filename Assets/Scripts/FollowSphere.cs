using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSphere : MonoBehaviour
{
    [SerializeField] GameObject domates;

    private void Update()
    {
        transform.position = new Vector3(0,0,domates.transform.position.z);
    }
}
