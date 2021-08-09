using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Player.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        private Text _text;
        private int _score = 0;
        private int _targetScore = 0;
        private LTDescr _tween;

        void Start()
        {
            _text = GetComponent<Text>();

            ScoreManager.Inst.OnScoreChanged += ScoreChangedHandler;
        }

        private void ScoreChangedHandler(int score)
        {
            _targetScore = score;

            if (_targetScore == _score) return;

            if (_tween != null)
            {
                LeanTween.cancel(_tween.id);
            }

            _tween = LeanTween.value(_score, _targetScore, 0.5f)
                .setOnUpdate(UpdateScore)
                .setOnComplete(() => _tween = null);
        }

        private void UpdateScore(float val)
        {
            _score = (int)val;
            _text.text = _score.ToString("D6");
        }
    }
}
