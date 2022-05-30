using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using GameControllerNameSpace;
using Unity.Ad.Interstýtýal;
using Unity.Services.Mediation;
using UnityEngine.UI;

public class CollectController : MonoBehaviour
{
    [SerializeField] Vector3 growthAmount;
    [SerializeField] float scoreIncreaseAmount, adPowerUpTime, graterShrinkFraction;

    [Header("Designations")]
    [SerializeField] ParticleSystem _particleCollect;
    [SerializeField] TMP_Text _scoretext;
    [SerializeField] GameObject _timerPanel;
    [SerializeField] TMP_Text _timerText;
    [SerializeField] Transform _transform;
    [SerializeField] Color _timerGoColor;
    [SerializeField] Color _timerNumberColor;
    [SerializeField] Image _adPowerUpImage;
    [SerializeField] AudioClip[] _audioClips;

    float score;
    Vector3 growedScale;
    Vector3 tempShrink;
    Vector3 graterShrinkVector;
    Vector3 tempTimerScale;
    InterstýtýalAd _ad;
    float cooldown;

    void Start()
    {
        AdSetUp();
        graterShrinkVector = new Vector3(graterShrinkFraction / 700, graterShrinkFraction / 700, graterShrinkFraction / 700);
        tempTimerScale = _timerText.transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollectableFruit"))
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
            StartCoroutine(AdPowerUp());
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.gameState = GameManager.GameState.Death;
            GameManager.Instance.Death();
            Debug.Log("Devam için Reklam önerisi gösterilecek veya oyun yeniden baþlayacak.");
            // State'in durumuna Can gitti veya kaybetti vs. de eklenip state ona çevrilince otomatik GameOver çýkmasý saðlanabilir.
        }
        if (other.gameObject.CompareTag("Grater"))
        {
            if (GameManager.isShrinking)
            {
                tempShrink = GameManager.Instance.shrinkAmount;
                GameManager.Instance.shrinkAmount += graterShrinkVector;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grater"))
        {
            GameManager.Instance.shrinkAmount = tempShrink;
        }
    }


    IEnumerator AdPowerUp()
    {
        if (_ad.ad.AdState == AdState.Loaded)
        {
            GameManager.gameState = GameManager.GameState.Paused;
            _ad.ShowAd();
            Time.timeScale = 0;
            SoundManager.Instance.PauseMusic();
        }

        yield return new WaitWhile(() => _ad.ad.AdState == AdState.Showing);

        Debug.Log("Reklam Bitti");
        Time.timeScale = 1;
        SoundManager.Instance.PlayMusic();

        #region AdCooldown
        Vector3 tempVelocity = new Vector3(0, 0, GameManager.Instance._rigidbody.velocity.z);
        GameManager.Instance._rigidbody.freezeRotation = true;
        GameManager.Instance._rigidbody.velocity = Vector3.zero;

        _timerPanel.SetActive(true);
        _timerText.transform.DOScale(5f, 0.3f).SetLoops(10, LoopType.Restart);
        for (int i = 3; i >= 1; i--)
        {
            _timerText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        _timerText.text = "GO!";
        _timerText.color = _timerGoColor;
        _timerText.transform.DOScale(5.5f, 1f);
        _timerText.DOColor(new Color(0, 0, 0, 0), 1f);

        yield return new WaitForSeconds(0.85f);
        #endregion

        GameManager.Instance._rigidbody.freezeRotation = false;
        GameManager.Instance._rigidbody.velocity = tempVelocity;
        GameManager.gameState = GameManager.GameState.Started;
        _timerPanel.SetActive(false);

        StartCoroutine(AdPowerUpCooldown());
        _adPowerUpImage.gameObject.SetActive(true);
        _adPowerUpImage.transform.DOScale(1.1f, 0.3f).SetLoops(10, LoopType.Yoyo);
        yield return new WaitForSeconds(adPowerUpTime);
        _adPowerUpImage.gameObject.SetActive(false);
        GameManager.isShrinking = true;

        _timerText.transform.localScale = tempTimerScale;
        _timerText.color = _timerNumberColor;
    }

    IEnumerator AdPowerUpCooldown()
    {
        cooldown = 0;

        while (true)
        {
            cooldown += Time.deltaTime;
            _adPowerUpImage.fillAmount = cooldown / adPowerUpTime;

            if (cooldown >= adPowerUpTime)
            {
                StopCoroutine(AdPowerUpCooldown());
            }

            yield return null;
        }
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
