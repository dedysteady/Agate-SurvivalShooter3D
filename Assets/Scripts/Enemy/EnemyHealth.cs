using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    void Awake ()
    {
        //Menapatkan reference komponen
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        //Set current health
        currentHealth = startingHealth;
    }

    void Update ()
    {
         //Check jika sinking
        if (isSinking)
        {
            //memindahkan object kebawah
            transform.Translate (sinkSpeed * Time.deltaTime * -Vector3.up);
        }
    }

    public void TakeDamage (int amount, Vector3 hitPoint)
    {
         //Check jika dead
        if (isDead)
            return;

        //play audio
        enemyAudio.Play ();

        //kurangi health
        currentHealth -= amount;

        //Ganti posisi particle
        hitParticles.transform.position = hitPoint;

        //Play particle system
        hitParticles.Play();

        //Dead jika health <= 0
        if (currentHealth <= 0)
        {
            Death ();
        }
    }

    void Death ()
    {
        //set isdead
        isDead = true;

        //SetCapcollider ke trigger
        BuffPlayer();
        capsuleCollider.isTrigger = true;

        //trigger play animation Dead
        anim.SetTrigger ("Dead");

        //Play Sound Dead
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

    public void StartSinking ()
    {
        //disable komponen navmesh
        GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;

        //Set rigisbody ke kimematic
        GetComponent<Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }

    //Menambah kuat player, baik dari attack, speed, atau health
    void BuffPlayer()
    {
        float random = Random.value;
        if (random > 0.95) //rate 5%
        {
            Debug.Log("Kecepatan player bertambah sebanyak 1");
            PlayerMovement.Instance.AddSpeed(1);
        }
        else if (random > 0.8) //rate 20%
        {
            PlayerHealth.Instance.AddHealth(5);
        }
        else if (random > 0.75) //rate 25%
        {
            Debug.Log("Attack player meningkat sebesar 20 dan recoil nya mengecil");
            PlayerShooting.Instance.IncreaseShooting(20, 0.01f);
        }    
    }
}
