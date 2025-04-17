using System.ComponentModel;
using UnityEngine;

public class ArmorBuff : MonoBehaviour
{
    public ItemData.ItemType itemType;
    public float rate;

    public void Init(ItemData itemData)
    {   
        name = "Buff" + itemData.id;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        itemType = itemData.itemType;
        // 버프들은 대미지가아닌 효과를 대미지에 넣어놨기 때문에 대미지 0 번으로 초기화
        rate = itemData.damages[0];

        // 0레벨일때 부터 버프 함수를 적용시켜야 하기때문에
        ApplyBuff();
    }

    public void BuffLevelUp(float rate)
    {
        // 현재 버프 등급을 알기 위해 레벨업 할때마다 갱신
        this.rate = rate;
        
        // 레벨업을 할때 적용시켜야 하기때문에
        ApplyBuff();
    }

    // 장갑,신발 버프 레벨업을 적용시켜주는 함수
    private void ApplyBuff()
    {
        switch(itemType)
        {
            case ItemData.ItemType.GLOVE:
                RateUp();
            break;
            case ItemData.ItemType.SHOE:
                SpeedUp();
            break;
        }
    }

    // 장갑 버프 - 연사력을 올리는 함수
    private void RateUp()
    {
        // Init함수에서 부모를 플레이어로 지정해주기 때문에 가능
        Weapon[] weapons = this.transform.parent.GetComponentsInChildren<Weapon>();
        
        // 하나의 무기가아닌 모든 무기적용
        foreach(var tmp in weapons)
        {   
            switch(tmp.itemId)
            {
                // 근접 무기일 때
                case 0:
                    // 레벨업 시 = 기본값(150) + (기본값(150) * 백분율)
                    float speed = 150 * Character.WeaponSpeed;
                    tmp.weaponSpeed = speed + (speed * rate);
                break;
                // 원거리 무기일 때
                default:
                    speed = 0.5f * Character.WeaponRate;
                    tmp.weaponSpeed = speed * (1 - rate);
                    break;
            } 
        }
    }

    // 신발 버프 - 이동속도를 올리는 함수
    private void SpeedUp()
    {
        // 플렝어 기본 이동속도
        float speed = 3f * Character.Speed;
        GameManager.Instance.player.playerSpeed = speed + speed * rate; 
    }
}
