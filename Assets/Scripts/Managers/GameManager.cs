using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Analytics;

namespace GameManagerNamespace
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
        Transform _playerTransform;

        public enum GameState { Started, Paused, Dead, Finished }
        public GameState gameState;

        [HideInInspector]
        public int currentFruitID = 0;

        [SerializeField] public FruitTypes[] fruitTypes;
        public static bool isShrinking;

        [Tooltip("Default: -9.81")]
        [SerializeField] float gravityScale;

        [Header("Designations")]
        [SerializeField] Image _gameOverPanel;
        [SerializeField] Image _playPanel;
        [SerializeField] Transform _blender;
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
            _playerTransform = _rigidbody.gameObject.transform;
        }

        private void FixedUpdate()
        {
            Roll();
        }

        #region GameState Methods
        public void Started()
        {
            gameState = GameState.Started;
            _rigidbody.freezeRotation = false;
            _rigidbody.velocity = AdManager.Instance.tempVelocity;
            PlayerControl.Instance.canMove = true;
        }

        public void Paused()
        {
            gameState = GameState.Paused;
            Time.timeScale = 0;
            PlayerControl.Instance.canMove = false;
            SoundManager.Instance.PauseMusic();
        }

        public void Dead()
        {
            gameState = GameState.Dead;
            _gameOverPanel.gameObject.SetActive(true);
            Time.timeScale = 0.5f;
        }

        public void Finished()
        {
            gameState = GameState.Finished;

            _rigidbody.AddForce((_blender.position - _playerTransform.position) / 2, ForceMode.VelocityChange);
        }
        #endregion

        void Roll()
        {
            if (gameState == GameState.Started && _playerTransform.localScale.x >= 0.1f && PlayerControl.Instance.canMove)
            {
                _rigidbody.AddForce(rollspeed * Vector3.forward, ForceMode.Acceleration);
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxRollSpeed);

                Vector3 tempScale = _playerTransform.localScale -= shrinkAmount; ;
                _playerTransform.DOScale(tempScale, 0.1f);

            }
            else if (_playerTransform.localScale.x < 0.1f)
            {
                Dead();
            }
        }

        public void Play()
        {
            StartCoroutine(StartGame());
        }

        public void Restart()
        {
            StopAllCoroutines();
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Unity Analytics | Custom event
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            AnalyticsService.Instance.CustomData("restartClicked", parameters);
        }

        public void NextLevel()
        {
            StopAllCoroutines();
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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


    }
}