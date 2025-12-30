using UnityEngine;
using static SoundManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ObservableProperty<int> score = new();

    // Voice
    public struct VoiceSet
    {
        public SFX startVoice;
        public SFX defeatVoice;
        public SFX scoreVoice1;
        public SFX scoreVoice2;

        public VoiceSet(SFX start, SFX defeat, SFX score1, SFX score2)
        {
            startVoice = start;
            defeatVoice = defeat;
            scoreVoice1 = score1;
            scoreVoice2 = score2;
        }
    }

    public VoiceSet[] voiceSets = new VoiceSet[]
    {
        new VoiceSet(SFX.StartVoice1, SFX.DefeatVoice1, SFX.ScoreVoice1, SFX.ScoreVoice2),
        new VoiceSet(SFX.StartVoice2, SFX.DefeatVoice2, SFX.ScoreVoice3, SFX.ScoreVoice4),
    };

    public int currentSetIndex = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UIManager.Instance.isMenuOpen.Subscribe(PauseControll);
        score.Value = 0;
        PlayStartVoice();
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
        score.Value += amount;
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            PlayScoreVoice1();
        }
        else
        {
            PlayScoreVoice2();
        }
    }

    #region Voice Methods
    // Voice Set 선택 메서드
    public VoiceSet SelectVoiceSet(int index)
    {
        if (index < 0 || index >= voiceSets.Length)
        {
            Debug.LogWarning("잘못된 VoiceSet 인덱스");
            return voiceSets[0]; // 기본값 반환
        }

        currentSetIndex = index;
        return voiceSets[index];
    }

    public void PlayStartVoice()
    {
        SoundManager.Instance.PlaySfx(voiceSets[currentSetIndex].startVoice);
    }

    public void PlayDefeatVoice()
    {
        SoundManager.Instance.PlaySfx(voiceSets[currentSetIndex].defeatVoice);
    }

    public void PlayScoreVoice1()
    {
        SoundManager.Instance.PlaySfx(voiceSets[currentSetIndex].scoreVoice1);
    }

    public void PlayScoreVoice2()
    {
        SoundManager.Instance.PlaySfx(voiceSets[currentSetIndex].scoreVoice2);
    }
    #endregion
}
