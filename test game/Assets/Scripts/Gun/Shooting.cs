using Unity.Mathematics;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public GameObject impactEffect;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

    }
    void OnEnable()
    {
        onFoot.Enable();
        onFoot.Shoot.performed += _ => Shoot();
    }
    void OnDisable()
    {
        onFoot.Disable();
    }
    void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            gunSound.PlayOneShot(gunSound.clip);
        }
    }
}
