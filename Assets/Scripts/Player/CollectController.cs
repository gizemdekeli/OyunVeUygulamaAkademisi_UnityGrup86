using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using GameControllerNameSpace;
using Unity.Ad.Interstýtýal;
using Unity.Services.Mediation;
using Unity.Services.Core;

public class CollectController : MonoBehaviour
{
    IMediationService Instance;
    [SerializeField] private Vector3 growthAmount;
    [SerializeField] private float scoreIncreaseAmount;
    [SerializeField] private TMP_Text scoretext;
    [SerializeField] private ParticleSystem particleCollect;
    [SerializeField] private float adPowerUpTime;
    public float score = 0;
    private Vector3 growedScale;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private InterstýtýalAd _ad;

    private void Awake()
    {
        _ad = new InterstýtýalAd();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;

        _ad.InitServices();
        _ad.SetupAd();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tomato"))
        {
            growedScale = _transform.localScale + growthAmount;
            _transform.DOScale(growedScale, 0.2f);
            other.gameObject.SetActive(false);
            score += scoreIncreaseAmount;
            scoretext.text = score.ToString();
            particleCollect.Play();
        }
        if (other.gameObject.CompareTag("Ad"))
        {
            other.gameObject.SetActive(false);
            GameController.isShrinking = false;
            StartCoroutine(AdPowerUp());
        }
    }

    IEnumerator AdPowerUp()
    {
        GameController.gameState = GameController.GameState.Paused;

        Time.timeScale = 0;
        _ad.ShowAd();

        yield return new WaitWhile(() => GameController.gameState == GameController.GameState.Paused);
        Debug.Log("Reklam Bitti");
        Time.timeScale = 1;

        yield return new WaitForSeconds(adPowerUpTime);
        GameController.isShrinking = true;
    }
}
