using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public string name;                 // 풀 이름
    public GameObject prefab;           // 생성 오브젝트
    public int size;                    // 풀 사이즈
    public Transform parentTransform;   // 부모 오브젝트
}
