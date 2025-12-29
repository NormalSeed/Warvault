using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        GameManager.Instance.score.Subscribe(OnScoreChanged);
    }

    void OnScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }
}
