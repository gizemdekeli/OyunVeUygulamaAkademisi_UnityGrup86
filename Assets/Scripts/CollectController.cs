using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectController : MonoBehaviour
{
    [Tooltip("Meyvenin Büyüme Miktarý")]
    [SerializeField] Vector3 growthAmount;

    Vector3 growedScale;
    Transform _transform;
    private void Start()
    {
        _transform = transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            growedScale = _transform.localScale + growthAmount;
            _transform.DOScale(growedScale, 0.2f);
            other.gameObject.SetActive(false);
        }
    }
}
