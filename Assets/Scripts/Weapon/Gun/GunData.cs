using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewGunData", menuName = "Weapon/GunData")]
public class GunData : ScriptableObject
{
    [Header("Basic info")]
    public string gunName; //枪械名称
    public GunType gunType; //枪械种类
    public Sprite sprite; //精灵
    [TextArea]
    public string description; //描述

    [Header("Shooting properties")]
    public int damage = 20; //伤害
    public float fireRate = .3f; //射击间隔
    public float range = 10f; //射程
    public float bulletSpeed = 20f; //子弹速度

    [Header("Accuracy")]
    public float baseSpread = 2f; //基础散布角度
    public float spreadIncreasePerShot = 1f; //每发增加的散步角度
    public float maxSpread = 10f; //最大散步角度
    public float spreadRecoverySpeed = 2f; //散步恢复速度

    [Header("Fire mode")]
    public FireMode fireMode; //开火模式

    [Header("Bullet")]
    public GameObject bulletPrefab; //子弹预制体
    public int bulletsPerShot = 1; //每一次射击的子弹量

    [Header("Gun model")]
    public GameObject gunPrefab;

    [Header("Layer mask")]
    public LayerMask hitLayerMask = -1; 
}
