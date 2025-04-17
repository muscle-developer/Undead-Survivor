using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // 왼손인지 체크하기 위한 변수
    public bool isLeftHand;
    // 방향전환을 위한 손 Sprite 
    public SpriteRenderer handSprite;
    // Player의 Sprite
    private SpriteRenderer playerSprite;

    // 오른손 전환
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0f);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0f);
    // 왼손 전환
    Quaternion leftRot = Quaternion.Euler(0f, 0f, -35f);
    Quaternion leftRotReverse = Quaternion.Euler(0f, 0f, -135f);

    void Awake()
    {
        playerSprite = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        // 플레이어 방향 확인
        bool isReverse = playerSprite.flipX;

        // 근접무기 일 때
        if(isLeftHand)
        {   
            // 플레이어가 왼 오 어느방향을 보고있는지에 따라 조정
            this.transform.localRotation = isReverse ? leftRotReverse : leftRot; 
            handSprite.flipY = isReverse;
            
            // Sprite의 Order In Layer수정 - 플레이어 스프라이트보다 앞인지 뒤인지 체크하기위해
            handSprite.sortingOrder = isReverse ? 4 : 6;
        }
        else // 원거리 일 때
        {
            this.transform.localPosition = isReverse ? rightPosReverse : rightPos;
            handSprite.flipX = isReverse;
            handSprite.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
