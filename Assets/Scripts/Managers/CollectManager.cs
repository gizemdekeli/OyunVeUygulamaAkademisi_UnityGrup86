using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using GameManagerNamespace;

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance = null;

    [SerializeField] Vector3 growthAmount;
    [SerializeField] float scoreIncreaseAmount, graterShrinkFraction;

    [Header("Designations")]
    [SerializeField] ParticleSystem _particleCollect;
    [SerializeField] TMP_Text _scoretext;
    [SerializeField] Transform _transform;
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] GameObject _timeline;

    Material _juice;

    [HideInInspector]
    public float collectedFruitCount;
    public float totalFruitCount;
    float score;
    Vector3 growedScale;
    Vector3 tempShrink;
    Vector3 graterShrinkVector;


    void Start()
    {
        _juice = Finish.Instance._juice.material;
        graterShrinkVector = new Vector3(graterShrinkFraction / 700, graterShrinkFraction / 700, graterShrinkFraction / 700);
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Started)
        {
            if (other.gameObject.CompareTag("CollectableFruit"))
            {
                other.gameObject.SetActive(false);
                GrowUp();
                collectedFruitCount++;
                score += scoreIncreaseAmount;
                ScoreToText();
                _juice.SetFloat("fillAmount", _juice.GetFloat("fillAmount") + (2 / totalFruitCount));
                SoundManager.Instance.PlaySoundEffect(_audioClips[Random.Range(0, _audioClips.Length)]);
                _particleCollect.Play();
            }


            if (other.gameObject.CompareTag("Ad"))
            {
                other.gameObject.SetActive(false);
                StartCoroutine(AdManager.Instance.AdPowerUpCollect());
            }


            if (other.gameObject.CompareTag("Obstacle"))
            {
                GameManager.Instance.Dead();
                Debug.Log("Devam için Reklam izlenip devam edilecek veya oyun yeniden baþlayacak.");
            }


            if (other.gameObject.CompareTag("Grater"))
            {
                if (GameManager.isShrinking)
                {
                    tempShrink = GameManager.Instance.shrinkAmount;
                    GameManager.Instance.shrinkAmount += graterShrinkVector;
                }
            }


            if (other.gameObject.CompareTag("Finish"))
            {
                GameManager.Instance.Finished();
                _timeline.SetActive(true);
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = new CollectManager();
        }
        else Destroy(gameObject);
    }
}
