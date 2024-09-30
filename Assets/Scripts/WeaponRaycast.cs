using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRaycast : MonoBehaviour
{
    [Header("Weapon Values")]
    public int damage = 10;
    public float weaponRange = 100f;
    public int maxAmmo = 20;

    public float rateOfFire;
    public float reloadTime;
    public bool isFullAuto;

    public float xSpread;
    public float ySpread;

    private float nextShootTime = 0f;
    private int currentAmmo;
    private bool isShooting, canShoot;
    private bool isReloading;

    private Ray laserRay;

    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] LayerMask damagableMask;

    private AudioSource audioSource;

    void Start()
    {
        //SETUP

        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //SET ORIGIN POSITION FOR LASER
        laserRenderer.SetPosition(0, Vector3.Slerp(laserRenderer.GetPosition(0), firePoint.position, 3f));

        //INPUT HANDLING
        if (isFullAuto)
            isShooting = Input.GetMouseButton(0);
        else
            isShooting = Input.GetMouseButtonDown(0);

        if (isShooting && !isReloading && currentAmmo > 0)
            Shoot();
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Reload());
    }
    private void Shoot()
    {
        //SET RAY ORIGIN
        Vector3 spread = new Vector3(Random.Range(-xSpread, xSpread), Random.Range(-ySpread, ySpread));

        //RENDER LASER WHEN SHOOTING (TARGET POSITION AND LENGHT) plus add spread
        laserRenderer.SetPosition(1, firePoint.position + (firePoint.forward * weaponRange) + spread);

        StartCoroutine(LaserRenderer());

        //PLAY AUDIO
        audioSource.Play();

        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, weaponRange, damagableMask))
        {
            //you hit something that is damagable
            if (hit.transform.TryGetComponent(out HittableObject hitObj))
            {
                hitObj.HitObjectWithForce(firePoint.forward, ForceMode.Impulse);
            }
        }

        //DECREMENT AMMO SLOWER IF GUN IS FULL AUTO
        if (isFullAuto && Time.time > nextShootTime)
        {
            nextShootTime += rateOfFire;
            currentAmmo--;
        }
        else
            currentAmmo--;
    }
    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        currentAmmo = maxAmmo;
    }
    private IEnumerator LaserRenderer()
    {
        //render laser based on weapon state
        if (!isFullAuto)
        {
            laserRenderer.enabled = true;

            yield return new WaitForSeconds(rateOfFire);

            laserRenderer.enabled = false;
        }
        else
        {
            //if is full auto
            if (isShooting)
            {
                laserRenderer.enabled = true;
                yield return new WaitForEndOfFrame();
            }
            laserRenderer.enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(laserRay.origin, laserRay.direction, Color.red);
     
    }
}
