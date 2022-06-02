using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.Services.Analytics;
using TMPro;

public class Finish : MonoBehaviour
{
    public static Finish Instance = null;

    [SerializeField] ParticleSystem _finishParticles;
    [SerializeField] Image _finishPanel;
    [SerializeField] Button _topRestartButton;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] public Renderer _juice;
    [SerializeField] CollectManager _collectManager;
    [SerializeField] AudioClip _mixerSound;
    [SerializeField] AudioClip _splashFX;

    float tempFillAmount;
    float fill = -1;
    float volume;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            _topRestartButton.gameObject.SetActive(false);
            SoundManager.Instance.PlaySoundEffect(_splashFX);

            DOTween.To(() => volume, x => volume = x, 0.2f, 7).OnUpdate(() =>
            {
                SoundManager.Instance.SetMusicVolume(volume);
            });
            _finishParticles.Play();

            _scoreText.text = $"{_collectManager.collectedFruitCount} / {_collectManager.totalFruitCount}";

            Fill();

            // Unity Analytics | Custom event
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            AnalyticsService.Instance.CustomData("finishedLevel", parameters);
        }
    }

    private void Fill()
    {
        tempFillAmount = _juice.material.GetFloat("fillAmount");
        _juice.material.SetFloat("fillAmount", -1);

        DOTween.To(() => fill, x => fill = x, tempFillAmount, 5).OnUpdate(() =>
        {
            _juice.material.SetFloat("fillAmount", fill);
        }).OnComplete(() => _finishPanel.transform.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutBack));

        SoundManager.Instance.PlaySoundEffect(_mixerSound);
        _juice.material.DOColor(new Color(1, 0.23f, 0, 1), 2);
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

