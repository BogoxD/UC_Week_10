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

    private int currentAmmo;
    private bool isShooting, canShoot;
    private bool isReloading;


    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] LayerMask damagableMask;

    void Start()
    {
        currentAmmo = maxAmmo;
    }
    private void Update()
    {
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
        Vector3 RayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        //RENDER LASER WHEN SHOOTING
        laserRenderer.SetPosition(0, Vector3.Slerp(laserRenderer.GetPosition(0), firePoint.position, 3f));
        laserRenderer.SetPosition(1, RayOrigin + (Camera.main.transform.forward * weaponRange));
        StartCoroutine(LaserRenderer());

        if (Physics.Raycast(RayOrigin, Camera.main.transform.forward, out RaycastHit hit, weaponRange, damagableMask))
        {
            //you hit something that is damagable
            
           
        }

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
}
