using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelUp : MonoBehaviour
{
    private RectTransform rect;
    private ItemLevelUp[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<ItemLevelUp>(true);
    }

    public void ShowLevelUpPopup()
    {
        // 활성화
        // this.gameObject.SetActive(true);
        RandomLevelUpEffect();
        rect.localScale = Vector3.one;
        GameManager.Instance.GameStop();
    }   

    public void HideLevelUpPopup()
    {
        // 비활성화
        // this.gameObject.SetActive(false);
        rect.localScale = Vector3.zero;
        GameManager.Instance.GameResume();
    }

    // 기본무기를 지급하는 함수
    public void BaseWeapon(int index)
    {   
        items[index].OnLevelUpButtonClicked();    
    }

    // 레벨업 시 무작위 아이템 보여주기
    private void RandomLevelUpEffect()
    {
        // 모든 아이템 비활성화
        foreach(var tmp in items)
            tmp.gameObject.SetActive(false);
        // 랜덤하게 3개의 아이템만 활성화
        int[] random = new int[3];
        // 3개의 배열의 들어가는 숫자는 중복되면 안되기 때문에 무한반복
        while(true)
        {
            // 3개의 배열에 랜덤한 숫자를 넣어주자
            random[0] = Random.Range(0, items.Length);
            random[1] = Random.Range(0, items.Length);
            random[2] = Random.Range(0, items.Length);

            // 3개의 배열에 각각 다른숫자가 들어오면 멈추자
            if (random[0] != random[1] &&
                random[1] != random[2] &&
                random[0] != random[2])
               break;
        }

        for(int i = 0; i < random.Length; i++)
        {
            ItemLevelUp randomItems = items[random[i]];
            
            // 아이템 레벨이 Max일 경우 소비아이템으로 대체
            if(randomItems.level == randomItems.data.damages.Length)
                items[4].gameObject.SetActive(true);
            else
                randomItems.gameObject.SetActive(true);
        }

    }
}
