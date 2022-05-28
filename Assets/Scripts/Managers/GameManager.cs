using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Core;
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

        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Transform _playerTransform;

        public enum GameState { Started, Paused, Death }
        public static GameState gameState;
        public static bool isShrinking;

        [Tooltip("Default: -9.81")]
        [SerializeField] float gravityScale;

        [Header("Definitions")]
        [SerializeField] Image _gameOverPanel;
        [SerializeField] Image _playPanel;
        [SerializeField] GameObject _player;
        [SerializeField] Image _topPanel;


        private void Awake()
        {
            Setup();
        }

        void Start()
        {
            Time.timeScale = 0;
            gameState = GameState.Paused;
        }

        private void FixedUpdate()
        {
            Roll();

        }

        public void Death()
        {
            _gameOverPanel.gameObject.SetActive(true);
            gameState = GameState.Paused;

            //_rigidbody.velocity = Vector3.zero;
            _rigidbody.freezeRotation = true;
        }

        void Roll()
        {
            if (gameState == GameState.Started && _playerTransform.localScale.x >= 0.1f)
            {
                _rigidbody.AddForce(rollspeed * Vector3.forward, ForceMode.Acceleration);
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxRollSpeed);
                if (isShrinking)
                {
                    _playerTransform.localScale -= Vector3.one / shrinkFraction;
                }
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
            _topPanel.gameObject.SetActive(true);
            _playPanel.gameObject.SetActive(false);
            Debug.Log("Game starts in 1 seconds");
            Time.timeScale = 1;
            yield return new WaitForSeconds(1);
            gameState = GameState.Started;
            isShrinking = true;
        }
    }
}