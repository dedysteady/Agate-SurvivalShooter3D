using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    #region Singleton
    private static PlayerShooting _instance = null;

    public static PlayerShooting Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerShooting>();

                if (_instance == null)
                {
                    Debug.LogError("Fatal Error: PlayerShooting not Found");
                }
            }

            return _instance;
        }
    }
    #endregion
    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 1.15f;        
    public float range = 100f;                      

    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                             
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;                                 
    float effectsDisplayTime = 0.2f;                

    void Awake()
    {
        //mengambil komponen mask
        shootableMask = LayerMask.GetMask("Shootable");

        //take component
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        //disable line rendere
        gunLine.enabled = false;

        //disable light
        gunLight.enabled = false;
    }

    public void Shoot()
    {
        timer = 0f;

        //play audio
        gunAudio.Play();

        //enable light
        gunLight.enabled = true;

        //play gun particle
        gunParticles.Stop();
        gunParticles.Play();

        //enable line rendere & set first position
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        //set posisi ray shoot & direction
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        //lakukan raycast, jika ketedeksi id enemy, hit apapun
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    public void IncreaseShooting(int plusDamage, float rate)
    {
        damagePerShot += plusDamage;
        if(timeBetweenBullets > 0.05)
            timeBetweenBullets -= rate;
    }
}