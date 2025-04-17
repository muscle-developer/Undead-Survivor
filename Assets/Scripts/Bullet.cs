using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 무기의 대미지
    public float damage;
    // 관통 변수
    public int per;

    // 총알의 속도를 위해
    private Rigidbody2D rigidbody2D; 

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // 초기화 함수
    public void  InitBullet(float damage, int per, Vector3 dir)
    {
        // 대미지와 관통
        this.damage = damage;
        this.per = per;

        float bulletSpeed = 15f;

        // 근접무기 관통은 -값, 그렇기에 관통되는 것들만 체크
        if(per >= 0)
        {
            // 총알의 속도를 주자
            rigidbody2D.velocity = dir * bulletSpeed;
        }
    }

    // 총알의 충돌감지 함수
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 적이 아니거나 OR 근접무기일 때 
        if(!collision.CompareTag("Enemy") || per == -100)
            return;

        // 적이 맞을수록 관통력은 내려가고 -값이 되면 사라진다.
        per--;        

        if(per < 0)
        {
            // 재활용을 위해 속도 초기화
            rigidbody2D.velocity = Vector2.zero;
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Area 이고 원거리무기일 때만 적용되게
        if(!collision.CompareTag("Area") || per == -100)
            return;   

        this.gameObject.SetActive(false);
    }
}
