using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public int Hp { get; private set; }
    public int Score { get; private set; }

    // Observable Properties
    public ObservableProperty<int> CurHp { get; private set; } = new();

    void Awake()
    {
        Hp = 100;
        Score = 100;
    }
}
