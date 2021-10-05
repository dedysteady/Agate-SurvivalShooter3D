using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Singleton
    private static PlayerMovement _instance = null;

    public static PlayerMovement Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerMovement>();

                if (_instance == null)
                {
                    Debug.LogError("Fatal Error: PlayerMovement not Found");
                }
            }

            return _instance;
        }
    }
    #endregion

    public float speed = 6f;
    Vector3 movemenet;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    private void Awake()
    {
        //mendapatkan nilai mask dari layer yang bernama Floor
        floorMask = LayerMask.GetMask("Floor");

        //Mendapatkan komponen Animator
        anim = GetComponent<Animator>();

        //Mendapatkan komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        float h = Input.GetAxisRaw("Horizontal");

        //Mendapatkan nilai input vertical (-1,0,1)
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    // Method Animating
    public void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

     // Method Turning
    void Turning()
    {
        //buat ray dari posisi mouse di layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

       //Buat raycast untuk floorHit
        if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, floorMask))
        {
            //ambil posisi vektor posisi player & floorHit
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;

            //ambil posisi look rotation baru ke hit position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            //rotasi player
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    //Method player jalan
    public void Move(float h, float v)
    {
        //set nilai x & y
        movemenet.Set(h, 0f, v);

        //normalisasi vector agar total panjang vector = 1
        movemenet = speed * Time.deltaTime * movemenet.normalized;

        //gerak ke posisi
        playerRigidbody.MovePosition(transform.position + movemenet);
    }

    public void AddSpeed(int amount)
    {
        if(speed < 20) speed += amount;
    }
}
