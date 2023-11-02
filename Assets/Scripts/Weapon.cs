using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기의 Id
    public int itemId;
    // 프리팹 Id
    public int prefabId;
    // 대미지
    public float weaponDamage;
    // 개수
    public int count;
    // 속도 
    public float weaponSpeed;

    void Start()
    {
        WeaponInit(); 
    }

    void Update()
    {
        // 각각의 아이템 종류에 따라 달라지는 회전값들
        switch(itemId)
        {
            case 0:
                // 근접 무기일때 회전할 수 있게
                this.transform.Rotate(Vector3.back * weaponSpeed * Time.deltaTime);
            break;
            case 1:

            break;
            default:

            break;
        }   

        // 레벨업에 따른 무기 소환을 위해 테스트 코드 작성
        if(Input.GetKeyDown(KeyCode.Space))
        {   
            // 대미지 - 20, 갯수 - 5
            WeaponLevelUp(20 ,5);
        }
    }

    // 무기 레벨업 함수
    public void WeaponLevelUp(float damage, int count)
    {
        this.weaponDamage = damage;
        this.count += count;

        if(itemId == 0)
            ArrangementWeapon();
    }

    // 초기화 함수
    public void WeaponInit()
    {
        // 각각의 아이템 종류에 따라 달라지는 초기값들
        switch(itemId)
        {
            // 근접 무기일 때
            case 0:
                weaponSpeed = 150;
                ArrangementWeapon();
            break;
            // 원거리 무기일 때
            case 1:

            break;
            // 그 이외에 기본 값
            default:

            break;
        }
    }

    // 생성된 근접 무기를 배치하는 함수
    private void ArrangementWeapon()
    {
        for(int i = 0; i < count; i++)
        {
            // 풀링을 해주기 위해 bullt이란 변수선언
            Transform bullet;

            // 현재 몇개의 무기가 생성되어있는지 체크  
            if(i < this.transform.childCount)
            {
                bullet = this.transform.GetChild(i);
            }
            // 보유하고 있는 무기보다 더 생성되어야 한다면 추가적으로 생성
            else
            {
                bullet = GameManager.Instance.poolManager.GetGameobject(prefabId).transform;
                // (Player - Weapon Sapwner에서 생성되어야 하기 떄문에 transform으로 지정) 
                bullet.parent = this.transform;
            }
            
            // 배치하기 전 위치,회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 생성되는 무기 갯수 만큼 알맞게 회전 및 배치
            Vector3 rotVector = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVector);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            // 여기서 대미지와 관통의 수치를 설정해주자(근접무기는 무조건 관통하기 때문에 -1값(∞)으로 설정)
            bullet.GetComponent<Bullet>().InitBullet(weaponDamage, -1);
        }
    }   
}
