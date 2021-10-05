using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Text warningText;
    public PlayerHealth playerHealth;       
    public float restartDelay = 8f;
    public bool isDead = false;
    Animator anim;                          
    float restartTimer;                    

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (playerHealth.currentHealth <= 0 && !isDead)
        {
            isDead = true;
            anim.SetTrigger("GameOver");
        }

        //Keluar dari Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Mengulang Game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowWarning(float enemyDistance)
    {
        //Memberi tanda warning ketika ada musuh mendekat
        warningText.text = string.Format("!\n{0} m", Mathf.RoundToInt(enemyDistance));
        anim.SetTrigger("Warning"); 
    }
}