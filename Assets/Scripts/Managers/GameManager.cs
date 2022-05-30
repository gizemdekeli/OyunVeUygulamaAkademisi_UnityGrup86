using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Analytics;

namespace GameControllerNameSpace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;
        [Header("Roll Controls")]
        [SerializeField] float rollspeed;
        [SerializeField] float maxRollSpeed;

        [Range(100, 1000)]
        [Tooltip("High Value -> Slower Shrinking \nRecommended 700")]
        [SerializeField] float shrinkFraction;

        [SerializeField] public Rigidbody _rigidbody;
        [SerializeField] Transform _playerTransform;

        public enum GameState { Started, Paused, Death }
        public static GameState gameState;

        public static bool isShrinking;

        [Tooltip("Default: -9.81")]
        [SerializeField] float gravityScale;

        [Header("Designations")]
        [SerializeField] Image _gameOverPanel;
        [SerializeField] Image _playPanel;
        [SerializeField] GameObject _player;
        [SerializeField] CanvasRenderer _topPanel;

        [HideInInspector]
        public Vector3 shrinkAmount;


        private void Awake()
        {
            Setup();
        }

        void Start()
        {
            Time.timeScale = 0;
            gameState = GameState.Paused;
            shrinkAmount = Vector3.one / shrinkFraction;
        }

        private void FixedUpdate()
        {
            Roll();
        }

        public void Death()
        {
            _gameOverPanel.gameObject.SetActive(true);
            gameState = GameState.Paused;

            _rigidbody.freezeRotation = true;
        }

        void Roll()
        {
            if (gameState == GameState.Started && _playerTransform.localScale.x >= 0.1f)
            {
                _rigidbody.AddForce(rollspeed * Vector3.forward, ForceMode.Acceleration);
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxRollSpeed);

                Vector3 tempScale = _playerTransform.localScale -= shrinkAmount; ;
                _playerTransform.DOScale(tempScale, 0.1f);

            }
            else if (_playerTransform.localScale.x < 0.1f)
            {
                Death();
            }
        }

        public void Play()
        {
            StartCoroutine(StartGame());
        }

        public void Restart()
        {
            StopAllCoroutines();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // For Custom event | Unity Analytics
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            AnalyticsService.Instance.CustomData("restartClicked", parameters);
        }

        private void Setup()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            Physics.gravity = new Vector3(0, gravityScale, 0);
            Application.targetFrameRate = 30;   //Default FrameRate for mobile
        }

        IEnumerator StartGame()
        {
            _playPanel.gameObject.SetActive(false);
            _topPanel.gameObject.SetActive(true);
            _topPanel.transform.DOLocalMoveY(_topPanel.transform.localPosition.y - 300, 0.7f).SetEase(Ease.OutBack);

            Time.timeScale = 1;
            Debug.Log("Game starts in 1 seconds");
            yield return new WaitForSeconds(1);
            gameState = GameState.Started;
            isShrinking = true;
        }
    }
}