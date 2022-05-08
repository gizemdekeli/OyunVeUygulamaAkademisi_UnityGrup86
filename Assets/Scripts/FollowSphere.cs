using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSphere : MonoBehaviour
{
    [SerializeField] GameObject tomato;

    private void Update()
    {
        transform.position = new Vector3(0, tomato.transform.position.y, tomato.transform.position.z);
    }
}
