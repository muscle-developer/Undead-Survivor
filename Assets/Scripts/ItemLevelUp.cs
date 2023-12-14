using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelUp : MonoBehaviour
{
    //레벨업에 필요한 정보
    public ItemData data;
    public int level;
    public Weapon weapon;
    public ArmorBuff armorBuff;

    Image iconimage;
    Text levelText;
    Text  nameText;
    Text descText;

    void Awake()
    {
        iconimage = GetComponentsInChildren<Image>()[1];
        iconimage.sprite = data.itemThumbnailImage;
        
        Text[] texts = GetComponentsInChildren<Text>();
        // 자식오브젝트 순서
        levelText = texts[0];
        nameText = texts[1];
        descText = texts[2];

        nameText.text = data.itemName;
    }

    // 오브젝트가 활성화 됐을 때 실행
    void OnEnable()
    {
        if (level == data.damages.Length)
            levelText.text = "Lv. Max";
        else
            levelText.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            // 근접,원거리 무기만 설명 두줄
            case ItemData.ItemType.MELEE:
            case ItemData.ItemType.RANGE:
                descText.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
            break;
            // 글러브, 신발 무기 설명 한줄
            case ItemData.ItemType.GLOVE:
            case ItemData.ItemType.SHOE:
                descText.text = string.Format(data.itemDesc, data.damages[level] * 100);
            break;   
            // 그 이외엔 값이 필요없음
            default:
                descText.text = string.Format(data.itemDesc);
            break;
        }
    }

    // 레벨업 버튼을 누르면 작동하는 이벤트 함수
    public void OnLevelUpButtonClicked()
    {   
        switch (data.itemType)
        {
            case ItemData.ItemType.MELEE:
            case ItemData.ItemType.RANGE:
                // level이 0일때는 무기를 생성시켜주자.
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.WeaponInit(data);
                }
                // Level이 0이 아닐 때는 레벨업을 시켜주자
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    // 다음 대미지 = 기본 공격력 + (기본공격력 * 다음 대미지) , 다음 대미지는 백분율이기때문에 기본공격력에 곱해준다.
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.WeaponLevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.GLOVE:
            case ItemData.ItemType.SHOE:
                // Level이 0일때 버프 초기값 세팅
                if(level == 0)
                {
                    GameObject newBuff = new GameObject();
                    armorBuff = newBuff.AddComponent<ArmorBuff>();
                    armorBuff.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    armorBuff.BuffLevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.HEAL:
                // 물약 버프 사용 시 맥스 HP로 설정
                GameManager.Instance.hp = GameManager.Instance.maxHP;
                break;
        }

        // 무기,버프의 레벨이 맥스치라면 레벨업 버튼 비활성화
        if (level == data.damages.Length)
            this.GetComponent<Button>().interactable = false;
    }
}
