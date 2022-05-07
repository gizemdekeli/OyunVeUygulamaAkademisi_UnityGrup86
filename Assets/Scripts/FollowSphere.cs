using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSphere : MonoBehaviour
{
    [SerializeField] GameObject sphere;

    private void Update()
    {
        transform.position = new Vector3(0,0,sphere.transform.position.z);
    }
}
