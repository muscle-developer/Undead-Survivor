using System;
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
    // 일정 시간마다 총알이 나가기 위한 타이머
    private float tiemr = 0f;

    void Update()
    {
        if(!GameManager.Instance.isLive)
            return;

        // 각각의 아이템 종류에 따라 달라지는 회전값들
        switch(itemId)
        {
            case 0:
                // 근접 무기일때 회전할 수 있게
                this.transform.Rotate(Vector3.back * weaponSpeed * Time.deltaTime);
            break;
            default:
                // 일정 시간마다 총알이 나갈 수 있도록
                tiemr += Time.deltaTime;
                if(tiemr > weaponSpeed)
                {
                    tiemr = 0f;
                    ShootWeapon();
                }
            break;
        }   
    }

    // 무기 레벨업 함수
    public void WeaponLevelUp(float damage, int count)
    {
        this.weaponDamage = damage;
        this.count += count;

        if(itemId == 0)
            ArrangementWeapon();

        GameManager.Instance.player.BroadcastMessage("ApplyBuff", SendMessageOptions.DontRequireReceiver);
    }

    // 초기화 함수
    public void WeaponInit(ItemData itemData)
    {
        // 기본값 세팅 (생성될 위치, 무기 이름)
        name = "Weapon" + itemData.id;
        transform.parent = GameManager.Instance.player.transform;
        // 플레이어 안에서 위치를 맞추기 떄문에 지역위치로 설정
        transform.localPosition = Vector3.zero;

        // 무기의 정보를 스크립터블 오브젝트에 있는 Data 정보로 갱신 (대미지, 갯수 등...)
        this.itemId = itemData.id;
        this.weaponDamage = itemData.baseDamage;
        this.count = itemData.baseCount;

        // 반복문을 통해 프리팹아이디를 찾자
        for (int i = 0; i < GameManager.Instance.poolManager.prefabs.Length; i++)
        {
            // PoolManager에 무기의 프리팹을 넣어놨으니 스크립터블 데이터의 프리팹과 같은지 확인 후 같으면 
            if(itemData.projectile == GameManager.Instance.poolManager.prefabs[i])
            {
                // 프리팹에 아이디를 찾을 수 있다.
                prefabId = i;
                break;
            }
        }

        // 각각의 아이템 종류에 따라 달라지는 초기값들
        switch(itemId)
        {
            // 근접 무기일 때
            case 0:
                weaponSpeed = 150 * Character.WeaponSpeed;
                ArrangementWeapon();
            break;
            // 원거리 무기일 때
            case 1:
                weaponSpeed = 0.5f * Character.WeaponRate;
            break;
            // 그 이외에 기본 값
            default:

            break;
        }

        // 손 세팅하기
        Hand hand  = GameManager.Instance.player.hands[(int)itemData.itemType];
        hand.handSprite.sprite = itemData.hand;
        hand.gameObject.SetActive(true);

        GameManager.Instance.player.BroadcastMessage("ApplyBuff", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().InitBullet(weaponDamage, -1, Vector3.zero);
        }
    }   

    // 총알을 발사하는 함수
    private void ShootWeapon()
    {
        // 플레이어와 가까이 있는 타겟을 감지했는지?
        if(!GameManager.Instance.player.scanner.nearestTarget)
            return;

        // 적의 위치와 방향
        Vector3 targetPosition = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPosition - this.transform.position;
        dir = dir.normalized;

        // 총알의 생성 위치 및 회전 지정
        Transform bullet = GameManager.Instance.poolManager.GetGameobject(prefabId).transform;
        bullet.position = this.transform.position;
        // 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // 총알에 대한 대미지 및 관통 수치 설정
        bullet.GetComponent<Bullet>().InitBullet(weaponDamage, count, dir);
    }
}
