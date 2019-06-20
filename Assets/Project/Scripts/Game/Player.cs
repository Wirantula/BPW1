using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public Camera playerCamera;

    [Header("Audio")]
    public GameObject reloadAudio;

    [Header("Gameplay")]
    public int initialHealth = 100;
    public int initialAmmo = 12;
    public float knockBackForce = 10f;
    public float hurtDuration = 0.5f;
    public float reloadDuration = 0.8f;

    private int health;
    public int Health { get { return health; } }

    private int ammo;
    public int Ammo { get { return ammo; } }

    private bool killed;
    public bool Killed { get { return killed; } }

    private bool isHurt;

    // Start is called before the first frame update
    void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0 && Killed == false){
            ammo--;
            GameObject bulletObject = ObjectPoolingManager.Intstance.GetBullet(true);
            bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.forward = playerCamera.transform.forward;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {

        if (otherCollider.gameObject.GetComponent<AmmoCrate>() != null)
        {
            //collect ammo
            AmmoCrate ammoCrate = otherCollider.gameObject.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;

            Destroy(ammoCrate.gameObject);
        }
        else if (otherCollider.gameObject.GetComponent<HealthCrate>() != null)
        {
            //collect health
            HealthCrate healthCrate = otherCollider.gameObject.GetComponent<HealthCrate>();
            health += healthCrate.health;

            Destroy(healthCrate.gameObject);
        }

            if (isHurt == false) {
            GameObject hazard = null;
            if (otherCollider.GetComponent<Enemy>() != null)
            {
                Enemy enemy = otherCollider.GetComponent<Enemy>();
                if (enemy.Killed == false)
                {
                    hazard = enemy.gameObject;
                    health -= enemy.damage;
                }
            } else if (otherCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = otherCollider.GetComponent<Bullet>();
                if(bullet.ShotByPlayer == false)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    bullet.gameObject.SetActive(false);
                }
            }
            if(hazard != null)
            {
                isHurt = true;
                //perform knockback
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockBackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockBackDirection, knockBackForce);

                StartCoroutine(HurtRoutine());
            }

            if(health <= 0)
            {
                if(killed == false)
                {
                    killed = true;
                    OnKill();
                }
            }
        } 
        
    }

    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtDuration);

        isHurt = false;
    }

    IEnumerator ReloadRoutine()
    {
        reloadAudio.SetActive(true);
        yield return new WaitForSeconds(reloadDuration);

        ammo = initialAmmo;
        reloadAudio.SetActive(false);
    }

    private void OnKill()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

    }
}
