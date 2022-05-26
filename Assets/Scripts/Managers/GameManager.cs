using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameControllerNameSpace
{
    public class GameManager : MonoBehaviour
    {
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
            Physics.gravity = new Vector3(0, gravityScale, 0);
            Application.targetFrameRate = 30;   //Default FrameRate for mobile
        }
        void Start()
        {
            Time.timeScale = 0;
            gameState = GameState.Paused;
        }

        private void FixedUpdate()
        {
            Roll();

            if (gameState == GameState.Death)   // BU KISIM DÜZENLENÝP DAHA VERÝMLÝ HALE GETÝRÝLEBÝLÝR
            {
                _gameOverPanel.gameObject.SetActive(true);
                _rigidbody.velocity = Vector3.zero;
            }
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
                gameState = GameState.Paused;
                _gameOverPanel.gameObject.SetActive(true);
            }
        }

        public void Play()
        {
            StartCoroutine(StartGame());
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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