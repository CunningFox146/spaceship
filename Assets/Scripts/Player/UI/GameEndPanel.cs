using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Asteroids.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Player.UI
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _score;
        [SerializeField] private RectTransform _highScore;
        [SerializeField] private RectTransform _newHighScore;
        [SerializeField] private RectTransform _anyKey;

        private CanvasGroup _canvas;

        public bool IsDone { get; private set; }

        void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        public void Init()
        {
            float duration = 0.5f;

            _canvas.alpha = 0f;

            _score.SetParent(transform);

            LeanTween.moveLocal(_score.gameObject, Vector3.zero, duration).setEase(LeanTweenType.easeInCubic);
            LeanTween.alphaCanvas(_canvas, 1f, duration)
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => Invoke("AnimateHighScore", 0.25f));
        }

        private void AnimateHighScore()
        {
            int highScore = ScoreManager.Inst.highScore;

            _highScore.gameObject.SetActive(true);
            var text = _highScore.GetComponent<Text>();
            if (highScore > 0)
            {
                LeanTween.value(0f, (float)highScore, 2f)
                    .setDelay(1f)
                    .setOnStart(()=> AudioManager.Inst.GetSound().Play("UI/ScoreFill"))
                    .setOnUpdate((float val) => text.text = $"HIGH SCORE {((int)val):D6}")
                    .setOnComplete(DisplayHighScoreInfo);
                return;
            }

            text.text = "NO HIGH SCORE";
            DisplayHighScoreInfo();
        }

        private void DisplayHighScoreInfo()
        {
            float highScore = ScoreManager.Inst.highScore;
            float score = ScoreManager.Inst.Score;
            bool isNewHighScore = score >= highScore;

            _newHighScore.gameObject.SetActive(true);
            _newHighScore.GetComponent<Text>().text =
                isNewHighScore
                    ? "NEW HIGH SCORE!"
                    : $"<color=red>{highScore - score}</color> POINTS LEFT";
            
            AudioManager.Inst.GetSound().Play(isNewHighScore ? "UI/HighScore" : "UI/NoHighScore");

            StartCoroutine(ShakeHighScore(0.5f));
            StartCoroutine(AnyKeyCoroutine());

            IsDone = true;
        }

        private IEnumerator ShakeHighScore(float duration)
        {
            var pos = _newHighScore.anchoredPosition;
            var time = Time.time;
            while (Time.time - time < duration)
            {
                _newHighScore.anchoredPosition = pos + Random.insideUnitCircle * 3f;
                yield return null;
            }

            _newHighScore.anchoredPosition = pos;
        }

        private IEnumerator AnyKeyCoroutine()
        {
            while (true)
            {
                _anyKey.gameObject.SetActive(!_anyKey.gameObject.activeSelf);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}