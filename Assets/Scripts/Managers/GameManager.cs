using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [Header("Other")]
        [SerializeField] GameObject _gameOverPanel;
        [SerializeField] GameObject _player;

        Rigidbody _rigidbody;
        Transform _playerTransform;

        public enum GameState { Started, Paused }
        public static GameState gameState;
        public static bool isShrinking;

        [Tooltip("Default: -9.81")]
        [SerializeField] float gravityScale;

        private void Awake()
        {
            _rigidbody = _player.GetComponent<Rigidbody>();
            _playerTransform = _player.GetComponent<Transform>();

            Physics.gravity = new Vector3(0, gravityScale, 0);
        }
        void Start()
        {
            gameState = GameState.Paused;
            StartCoroutine(StartGame());
        }

        private void FixedUpdate()
        {
            Roll();
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
                _gameOverPanel.SetActive(true);
            }
        }

        // Oyun baþladýktan 2 saniye sonra kontrol ve dönme baþlýyor. Play butonu eklenince kontrol butona baðlanacak.
        // "IEnumerator StartGame()" yerine Butonun Play metodu geçecek.
        IEnumerator StartGame()
        {
            Debug.Log("Game starts in 3 seconds");
            yield return new WaitForSeconds(3);
            gameState = GameState.Started;
            isShrinking = true;
        }
    }
}