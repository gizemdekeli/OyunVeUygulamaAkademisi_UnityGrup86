using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerNamespace;
using Unity.Ad.Interstýtýal;
using Unity.Services.Mediation;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class AdManager : MonoBehaviour
{
    InterstýtýalAd _ad;
    public static AdManager Instance = null;

    [SerializeField] Image _timerPanel;
    [SerializeField] TMP_Text _timerText;
    [SerializeField] Color _timerGoColor;
    [SerializeField] Color _timerNumberColor;
    [SerializeField] Image _adPowerUpImage;
    [SerializeField] GameObject _shield;
    [SerializeField] float adPowerUpTime;

    [HideInInspector]
    public Vector3 tempVelocity;
    Vector3 tempTimerScale;

    float cooldown;

    void Start()
    {
        AdSetUp();
        tempTimerScale = _timerText.transform.localScale;
        _timerText.color = _timerNumberColor;
    }

    public IEnumerator AdPowerUpCollect()
    {
        if (_ad.ad.AdState == AdState.Loaded)
        {
            GameManager.Instance.Paused();
            _ad.ShowAd();
        }

        yield return new WaitWhile(() => GameManager.Instance.gameState == GameManager.GameState.Paused);

        Debug.Log("Reklam Bitti");
        _shield.SetActive(true);
        Time.timeScale = 1;
        SoundManager.Instance.PlayMusic();

        tempVelocity = new Vector3(0, 0, GameManager.Instance._rigidbody.velocity.z);
        GameManager.Instance._rigidbody.freezeRotation = true;
        GameManager.Instance._rigidbody.velocity = Vector3.zero;

        #region AdCooldown
        _timerPanel.gameObject.SetActive(true);
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

        GameManager.Instance.Started();
        _timerPanel.gameObject.SetActive(false);

        if (GameManager.Instance.gameState == GameManager.GameState.Started)
        {
            GameManager.isShrinking = false;
            StartCoroutine(AdPowerUpCooldown());
            _adPowerUpImage.gameObject.SetActive(true);
            _adPowerUpImage.transform.DOScale(1.1f, 0.3f).SetLoops(10, LoopType.Yoyo);
            yield return new WaitForSeconds(adPowerUpTime);
            _adPowerUpImage.gameObject.SetActive(false);
            GameManager.isShrinking = true;

            _timerText.transform.localScale = tempTimerScale;
            _timerText.color = _timerNumberColor;
            _shield.SetActive(false);
        }
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
 
    void AdSetUp()
    {
        _ad = new InterstýtýalAd();
        _ad.InitServices();
        _ad.SetupAd();
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
