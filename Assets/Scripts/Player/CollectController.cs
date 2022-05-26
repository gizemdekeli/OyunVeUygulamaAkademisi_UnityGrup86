using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using GameControllerNameSpace;
using Unity.Ad.Interstýtýal;
using Unity.Services.Mediation;

public class CollectController : MonoBehaviour
{
    [SerializeField] Vector3 growthAmount;
    [SerializeField] float scoreIncreaseAmount;
    [SerializeField] float adPowerUpTime;
    [SerializeField] TMP_Text _scoretext;
    [SerializeField] ParticleSystem _particleCollect;
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] Transform _transform;

    float score;
    Vector3 growedScale;
    InterstýtýalAd _ad;

    void Start()
    {
        AdSetUp();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tomato"))
        {
            other.gameObject.SetActive(false);
            GrowUp();

            score += scoreIncreaseAmount;
            ScoreToText();

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

        if (_ad.ad.AdState==AdState.Loaded)
        {
            GameManager.gameState = GameManager.GameState.Paused;
            _ad.ShowAd();
            Time.timeScale = 0;
            SoundManager.Instance.PauseMusic();
        }

        yield return new WaitWhile(() => GameManager.gameState == GameManager.GameState.Paused);
        Debug.Log("Reklam Bitti");
        Time.timeScale = 1;
        SoundManager.Instance.PlayMusic();

        yield return new WaitForSeconds(adPowerUpTime);
        GameManager.isShrinking = true;
    }

    void ScoreToText()
    {
        _scoretext.text = score.ToString();
        _scoretext.DOColor(Color.red, .2f).OnComplete(() => _scoretext.DOColor(Color.black, .2f));
        _scoretext.transform.DOScale(Vector3.one * 1.5f, .1f).OnComplete(() => _scoretext.transform.DOScale(Vector3.one, .1f));
    }

    void GrowUp()
    {
        growedScale = _transform.localScale + growthAmount;
        _transform.DOScale(growedScale, 0.1f);
    }

    void AdSetUp()
    {
        _ad = new InterstýtýalAd();
        _ad.InitServices();
        _ad.SetupAd();
    }
}
