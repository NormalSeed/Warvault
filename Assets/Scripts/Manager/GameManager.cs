using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UIManager.Instance.isMenuOpen.Subscribe(PauseControll);
        score = 0;
    }

    void Update()
    {

    }

    void PauseControll(bool isOpened)
    {
        if (isOpened)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
