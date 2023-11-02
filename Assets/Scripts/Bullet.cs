using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알의 대미지
    public float damage;
    // 관통 변수
    public int per;

    // 초기화 함수
    public void InitBullet(float damage, int per)
    {
        // 대미지와 관통
        this.damage = damage;
        this.per = per;
    }
}
