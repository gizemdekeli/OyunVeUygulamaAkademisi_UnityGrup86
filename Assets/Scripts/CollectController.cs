using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectController : MonoBehaviour
{
    private Vector3 tempscale;
    private float temppos;
    [SerializeField] private float growth;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            tempscale = gameObject.transform.localScale;
            tempscale.x += growth/100;
            tempscale.y += growth/100;
            tempscale.z += growth/100;
            gameObject.transform.localScale = tempscale;

            Destroy(other.gameObject);
        }
    }
}
