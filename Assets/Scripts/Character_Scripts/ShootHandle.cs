using UnityEngine;
using System.Collections;
using TMPro;
public class ShootHandle : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public float spread;

    public int maxAmmo = 45;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpscam;
    public ParticleSystem MuzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public Animator animator;
    public TextMeshProUGUI text;
    public RaycastHit rayHit;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
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
        MuzzleFlash.Play();

        currentAmmo--;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpscam.transform.forward + new Vector3(x, y, 0);

        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
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

            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 1f);
        }
    }
}
