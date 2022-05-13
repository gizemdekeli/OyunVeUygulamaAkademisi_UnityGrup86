using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControllerNameSpace
{
    public class GameController : MonoBehaviour
    {
        [Header("Roll Controls")]
        [SerializeField] float rollspeed;
        [SerializeField] float maxRollSpeed;

        [Range(1, 1000)]
        [Tooltip("High Value -> Slower Shrinking \nRecommended 700")]
        [SerializeField] float shrinkFraction;

        [Header("Other")]
        [SerializeField] GameObject gameOverPanel;
        [SerializeField] GameObject player;

        Rigidbody _rigidbody;
        Transform playerTransform;

        public enum GameState { Started, Paused }
        public static GameState gameState;

        private void Awake()
        {
            _rigidbody = player.GetComponent<Rigidbody>();
            playerTransform = player.GetComponent<Transform>();
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
            if (gameState == GameState.Started && playerTransform.localScale.x >= 0.1f)
            {
                _rigidbody.AddForce(rollspeed * Vector3.forward, ForceMode.Acceleration);
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxRollSpeed);
                playerTransform.localScale -= Vector3.one / shrinkFraction;
            }
            else if (playerTransform.localScale.x < 0.1f)
            {
                gameState = GameState.Paused;
                gameOverPanel.SetActive(true);
            }
        }

        // Oyun baþladýktan 2 saniye sonra kontrol ve dönme baþlýyor. Play butonu eklenince kontrol butona baðlanacak.
        // "IEnumerator StartGame()" yerine Butonun Play metodu geçecek.
        IEnumerator StartGame()
        {
            Debug.Log("Game starts in 3 seconds");
            yield return new WaitForSeconds(3);
            gameState = GameState.Started;
        }
    }
}