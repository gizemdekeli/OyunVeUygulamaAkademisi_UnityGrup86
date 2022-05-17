using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CollectController : MonoBehaviour
{
    [SerializeField] private Vector3 growthAmount;
    [SerializeField] private float scoreIncreaseAmount;
    [SerializeField] private TMP_Text scoretext;
    [SerializeField] private ParticleSystem particleCollect;
    public float score = 0;
    private Vector3 growedScale;
    private Transform _transform;

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
            score += scoreIncreaseAmount;
            scoretext.text = score.ToString();
            particleCollect.Play();
        }
    }
}
