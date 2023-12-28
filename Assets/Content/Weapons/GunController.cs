using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public KeyCode FireKey;

    public GunClass[] ClassLibary;
    public GunClass Class;
    public int classIndex;

    public string Name;
    private float fireRate;
    private float reloadTime;
    private int damage;
    private float recoil;
    private float shake;
    private bool Automatic;
    private float bulletSpeed;

    private ParticleSystem muzzleFlash;
    private AudioClip sound;
    private GameObject bullet;

    private GameObject WeaponPrefab;
    public Transform end;

    private float nextFireTime = 0f;

    private void Start()
    {
        UpdateClass(0);
    }

    public void UpdateClass(int index)
    {
        Class = ClassLibary[index];
        fireRate = Class.fireRate;
        reloadTime = Class.reloadTime;
        damage = Class.damage;
        recoil = Class.recoil;
        shake = Class.shake;
        Automatic = Class.Automatic;
        bulletSpeed = Class.bulletSpeed;

        //muzzleFlash = Class.muzzleFlash;
        //sound = Class.sound;
        bullet = Class.bullet;

        WeaponPrefab = Class.WeaponPrefab;
        end = GameObject.FindWithTag("End").transform;
    }

    private void Update()
    {
        if (Automatic)
        {
            if (Input.GetKeyDown(FireKey) && Time.time > nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            // Check for automatic firing
            if (Input.GetKey(FireKey) && Time.time > nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Fire()
    {
        GameObject obj = Instantiate(bullet, end.position, end.rotation);
        obj.GetComponent<Rigidbody>().AddForce(end.transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(obj, 10);

        //AudioSource.PlayClipAtPoint(sound, transform.position);


        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        // Apply recoil force
        transform.Translate(Vector3.back * recoil);
    }

}
