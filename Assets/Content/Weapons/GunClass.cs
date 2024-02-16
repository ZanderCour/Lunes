using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon System/Weapon")]
public class GunClass : ScriptableObject
{
    [SerializeField] public string ItemName;
    public float fireRate;
    public float reloadTime;
    public int damage;
    public Vector3 recoilPattern;
    public float shake;
    public bool Automatic;
    public float bulletSpeed;

    public ParticleSystem muzzleFlash;
    public AudioClip sound;
    public GameObject bullet;

    public GameObject WeaponPrefab;

    public enum WeaponType
    {
        Pistol,
        Rifle,
        Sword,
        Meele
    };

    public WeaponType type = new WeaponType();
}
