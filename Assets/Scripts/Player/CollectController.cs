using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using GameControllerNameSpace;
using Unity.Ad.Interstýtýal;

public class CollectController : MonoBehaviour
{
    [SerializeField] private Vector3 growthAmount;
    [SerializeField] private float scoreIncreaseAmount;
    [SerializeField] private float adPowerUpTime;
    [SerializeField] private TMP_Text _scoretext;
    [SerializeField] private ParticleSystem _particleCollect;
    [SerializeField] private AudioClip[] _audioClips;

    private float score;
    private Vector3 growedScale;
    private Transform _transform;
    private InterstýtýalAd _ad;

    private void Start()
    {
        _transform = transform;
        AdSetUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tomato"))
        {
            other.gameObject.SetActive(false);
            growedScale = _transform.localScale + growthAmount;
            _transform.DOScale(growedScale, 0.1f);

            score += scoreIncreaseAmount;
            _scoretext.text = score.ToString();

            SoundManager.Instance.PlaySoundEffect(_audioClips[Random.Range(0, _audioClips.Length)]);
            _particleCollect.Play();
        }
        if (other.gameObject.CompareTag("Ad"))
        {
            other.gameObject.SetActive(false);
            GameManager.isShrinking = false;
            StartCoroutine(AdPowerUp());
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.gameState = GameManager.GameState.Death;
            Debug.Log("Can gidecek veya oyun yeniden baþlayacak. Ayarlanmasý yapýlýr.");
            // State'in durumuna Can gitti veya kaybetti vs. de eklenip state ona çevrilince otomatik GameOver çýkmasý saðlanabilir.
        }
    }

    IEnumerator AdPowerUp()
    {
        GameManager.gameState = GameManager.GameState.Paused;

        _ad.ShowAd();

        yield return new WaitWhile(() => GameManager.gameState == GameManager.GameState.Paused);
        Debug.Log("Reklam Bitti");

        yield return new WaitForSeconds(adPowerUpTime);
        GameManager.isShrinking = true;
    }

    private void AdSetUp()
    {
        _ad = new InterstýtýalAd();
        _ad.InitServices();
        _ad.SetupAd();
    }
}
