using System.Collections;
using TMPro;
using UnityEngine;

public class ShotgunHandle : MonoBehaviour
{
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime,timebetweenShots;
    public int magazineSize, bulletsperTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask WhatIsEnemy;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsperTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x,y,0);

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, WhatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);


            //Puede tener errores por enemigo
            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<Target>().TakeDamage(damage);
        }

        bulletsLeft--;
        bulletsShot--;


        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timebetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished",reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading= false;
    }
}