using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GunController : MonoBehaviour
{
    // Gun attributes
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    // Shooting attributes
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;

    // Effects
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce = 30f;

    // UI
    public Text ammoText;

    // Audio
    public AudioSource gunAudioSource;
    public AudioClip shootClip;
    public AudioClip reloadClip;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        //gunAudioSource.PlayOneShot(reloadClip);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        yield return new WaitForSeconds(0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoText();
    }

    void Shoot()
    {
        //muzzleFlash.Play();
        //gunAudioSource.PlayOneShot(shootClip);
        currentAmmo--;
        UpdateAmmoText();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    void UpdateAmmoText()
    {
        ammoText.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }
}
