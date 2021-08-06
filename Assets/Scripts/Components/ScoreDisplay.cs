using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Components
{
    public class ScoreDisplay : MonoBehaviour
    {
        private Text _text;

        void Start()
        {
            _text = GetComponent<Text>();

            ScoreManager.Inst.OnScoreChanged += ScoreChangedHandler;
        }

        private void ScoreChangedHandler(int score)
        {
            _text.text = score.ToString();
        }
    }
}
