using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunController : MonoBehaviour
{
    private DebugConsole Console;

    [Header("Settings")]
    public KeyCode FireKey;
    public float recoilLerpSpeed = 5f;
    public Transform gunRoot;
    public Camera cam;
    public int MaxLoadoutCapasity = 3;

    public CinemachineImpulseSource impulseSource;

    public GameObject target;
    public GameObject hitPrefab;

    public GunClass[] ClassLibary;
    public GunClass HeldItem;
    public int classIndex;
    public List<GunClass> ActiveItems = new List<GunClass>();

    public string Name;
    private float fireRate;
    private float reloadTime;
    private int damage;
    private Vector3 recoilPattern;
    private float shake;
    private bool Automatic;
    private bool Hitscan;
    private float bulletSpeed;

    private ParticleSystem muzzleFlash;
    private AudioClip sound;
    private GameObject bullet;
    private GameObject WeaponPrefab;

    private float nextFireTime = 0f;
    public bool isShooting;
    bool canShoot;
    private Quaternion accumulatedRecoilRotation = Quaternion.identity;


    private void Start()
    {
        Console = FindObjectOfType<DebugConsole>();

        AddItem(0);
        AddItem(1);
    }

    public void UpdateHeldItem(int index)
    {
        HeldItem = ActiveItems[index];
        Name = HeldItem.ItemName;
        fireRate = HeldItem.fireRate;
        reloadTime = HeldItem.reloadTime;
        damage = HeldItem.damage;
        recoilPattern = HeldItem.recoilPattern;
        shake = HeldItem.shake;
        Automatic = HeldItem.Automatic;
        bulletSpeed = HeldItem.bulletSpeed;
        Hitscan = HeldItem.HitScan;
        //muzzleFlash = Class.muzzleFlash;
        //sound = Class.sound;
        bullet = HeldItem.bullet;

        WeaponPrefab = HeldItem.WeaponPrefab;

        Console.WriteMessage("Updated held item " + "Name: " + Name + " Type:" + HeldItem.weaponType);
    }

    public void AddItem(int index)
    {
        ActiveItems.Add(ClassLibary[index]);
    }

    private void Update()
    {
        isShooting = Input.GetKey(FireKey);
        canShoot = Time.time > nextFireTime;


        PlayerController player = GetComponent<PlayerController>();
        if (player.activeItem == PlayerController.ActiveItem.hands)
            return;

        if (!player.CanControll)
            return;

        if (Automatic)
        {
            if (Input.GetKeyDown(FireKey) && canShoot)
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetKey(FireKey) && canShoot)
            {
                Fire();
            }
        }

        HandleTarget();
        HandleRecoil();

    }

    private void HandleTarget()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            target.transform.position = raycastHit.point;
        }
    }

    private void Fire()
    {
        Vector3 shootingDirection = gunRoot.forward;

        GameObject projectile = Instantiate(bullet, gunRoot.position, gunRoot.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = shootingDirection * bulletSpeed;

        Bullet bulletScript = projectile.GetComponent<Bullet>();
        bulletScript.HitScan = Hitscan;
        bulletScript.Damage = damage;

        if (Hitscan)
        {
            Ray ray = new Ray(gunRoot.position, shootingDirection);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.transform.gameObject.GetComponent<Health>())
                {
                    Health health = raycastHit.transform.gameObject.GetComponent<Health>();
                    health.TakeDamage(damage);
                }
            }
        }

        nextFireTime = Time.time + fireRate;
    }

    private void HandleRecoil()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, gunRoot.transform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            Debug.DrawRay(gunRoot.transform.position, gunRoot.forward * hit.distance, Color.magenta);


        if (isShooting && canShoot) 
        {
            // Apply recoil rotation on the gunRoot
            float recoilPitch = Random.Range(-recoilPattern.x, recoilPattern.x);
            float recoilYaw = Random.Range(-recoilPattern.y, recoilPattern.y);
            float recoilRoll = Random.Range(-recoilPattern.z, recoilPattern.z);

            Quaternion recoilRotation = Quaternion.Euler(recoilPitch, recoilYaw, recoilRoll);
            accumulatedRecoilRotation *= recoilRotation;

            accumulatedRecoilRotation = Quaternion.Lerp(accumulatedRecoilRotation, Quaternion.identity, Time.deltaTime * recoilLerpSpeed);

            float recoilForce = Random.Range(shake, shake + 0.75f);
            Vector3 totalRecoilForce = recoilForce * accumulatedRecoilRotation.eulerAngles.normalized;
            // Generate impulse with force on the gunRoot
            impulseSource.GenerateImpulseWithVelocity(totalRecoilForce);
        }
        else if(!isShooting) 
        {
            accumulatedRecoilRotation = Quaternion.Lerp(accumulatedRecoilRotation, Quaternion.identity, Time.deltaTime * recoilLerpSpeed);
        }

        gunRoot.localRotation = accumulatedRecoilRotation;

    }
}
