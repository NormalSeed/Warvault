using UnityEngine;

public class PlayerModel
{
    public float MoveSpeed { get; set; } = 2f;
    public float Gravity { get; set; } = -9.81f;
    public float JumpHeight { get; set; } = 0.3f;

    public Vector3 Velocity = Vector3.zero;

    // Player 스탯
    public float Hp { get; set; } = 100f;
    
}
