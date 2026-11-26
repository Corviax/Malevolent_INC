using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
public class ShootRifleHandle : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public float spread;

    public int pelletsPerShot;
    public int maxAmmo = 45;
    private int bulletsShot;
    private int currentAmmo;
    private int AmmoShot;
    public float reloadTime = 1f;
    private bool isReloading = false;

    //int bulletsLeft, bulletsShot;

    public Camera fpscam;
    public ParticleSystem MuzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public Animator animator;
    public TextMeshProUGUI text;
    public RaycastHit rayHit;

    bool readyToShoot;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        readyToShoot = true;
        isReloading = false;
        animator.SetBool("Reloading",false);
    }

    void Update()
    {
        if(isReloading)
            return;

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1")&& Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }

        text.SetText(currentAmmo + "/" + maxAmmo);

        if (readyToShoot && isReloading && currentAmmo > 0)
        {
            bulletsShot = pelletsPerShot;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading",true);

        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
    void Shoot()
    {
        readyToShoot = false;

        MuzzleFlash.Play();

        currentAmmo--;
        bulletsShot--;


        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpscam.transform.forward + new Vector3(x, y, 0);

        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null )
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 1f);
        }

        if (bulletsShot > 0 && currentAmmo > 0)
            Invoke("Shoot", fireRate);
    }
}
