using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{   
    // Enum 열거형으로 선언(관리할 데이터들)
    public enum InfoType { EXP, LEVEL, KILL, TIME, HP}
    private InfoType[] type = new InfoType[5];
    [SerializeField]
    private Slider expSlider;
    [SerializeField]
    private Text playerLevelText;
    [SerializeField]
    private Text killScoreText;
    [SerializeField]
    private Text playTimeText;
    [SerializeField]
    private Slider hpSlider;

    void Awake()
    {
        if(expSlider != null)
            type[0] = InfoType.EXP;
        if(playerLevelText != null)
            type[1] = InfoType.LEVEL;
        if(killScoreText != null)
            type[2] = InfoType.KILL;
        if(playTimeText != null)
            type[3] = InfoType.TIME;
        if(hpSlider != null)
            type[4] = InfoType.HP;
    }

    // 추후에 LateUpdate -> Refresh()함수로 변경 후 업데이트에서 계속 호출하지말고, 예를들어 경험치를 획득할 때, 킬을 할때 호출하도록 변경

    // 모든 Update 함수가 호출된 후, 마지막으로 호출한다.
    void LateUpdate()
    {
        foreach(var tmp in type)
        {
            switch (tmp)
            {
                case InfoType.EXP:
                    float currentExp = GameManager.Instance.exp;
                    float maxExp = GameManager.Instance.nextExp[Mathf.Min(GameManager.Instance.level, GameManager.Instance.nextExp.Length - 1)];

                    // 현재 경험치 / 최대 경험치 
                    expSlider.value = currentExp / maxExp;
                break;
                case InfoType.LEVEL:
                    playerLevelText.text = "Lv." + GameManager.Instance.level.ToString();
                break;
                case InfoType.KILL:
                    killScoreText.text = "KILL SCORE:" + GameManager.Instance.kill.ToString();
                break;
                case InfoType.TIME:
                    // 남은 시간 구하기 (최대 플레이시간 - 현재 플레이 시간)
                    float remainTime = GameManager.Instance.maxPlayTime - GameManager.Instance.PlayTiem;
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);
                    
                    // 시간을 보여주는 텍스트에 분과 초를 보여준다
                    playTimeText.text = string.Format("{0:D2} : {1:D2}", min, sec);
                break;
                case InfoType.HP:
                    float currentHP = GameManager.Instance.hp;
                    float maxHP = GameManager.Instance.maxHP;
                    hpSlider.value = currentHP / maxHP;
                break;
            }
        }
    }
}
