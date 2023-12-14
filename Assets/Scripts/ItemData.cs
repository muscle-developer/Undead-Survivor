using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    // 아이템 타입 - 근접, 원거리, 이동속도 등등..
    public enum ItemType { MELEE, RANGE, GLOVE, SHOE, HEAL }

    // 무기 이름, 속성, 설명 등...
    [Header("Main Info")]  
    public ItemType itemType;
    public int id;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemThumbnailImage;

    // 레벨별로 상승하는 능력치
    [Header("Level Data")]
    // 기본(0레벨) 대미지, 근접무기 갯수
    public float baseDamage;
    public int baseCount;
    // 레벨업에 따른 대미지, 갯수
    public float[] damages;
    public int[] counts;

    [Header("Weapon")]
    // 투사체 오브젝트
    public GameObject projectile;
    // 각 무기에 맞게 데이터 생성을 위해
    public Sprite hand;
}
