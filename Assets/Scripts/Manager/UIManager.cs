using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject menuUI;

    public ObservableProperty<bool> isMenuOpen = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        menuUI.SetActive(false);
        isMenuOpen.Value = false;
        isMenuOpen.Subscribe(OnMenuOpened);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen.Value = !isMenuOpen.Value;
        }
    }

    void OnMenuOpened(bool isOpened)
    {
        menuUI.SetActive(isOpened);
    }
}
