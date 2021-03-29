using UnityEngine;
using System.Collections;
using TMPro;

public class MyPlayerController : MonoBehaviour
{

    Rigidbody rb;
    public static float speed;
    public bool isGameStart = true;
    public bool speedDown;
    public TextMeshProUGUI gameOverTxt;
    float rotationSpeed = 100.0F;
    Animator animator;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idling", true);
        gameOverTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isGameStart)
        {
            Move();
            Sprint();
            Slow();
            SlowSpeed();
        }
    }
    void Move()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (translation != 0)
        {
            animator.SetBool("Idling", false);
            this.GetComponent<SetupLocalPlayer>().CmdChangeAnimation("run");
        }
        else
        {
            animator.SetBool("Idling", true);
            this.GetComponent<SetupLocalPlayer>().CmdChangeAnimation("idle");
        }
    }
    void Sprint()
    {
        if (Input.GetKey(KeyCode.X))
        {
            if (StaminaBar.instance.currentStamina > 20)
            {
                StaminaBar.instance.UseStamina(0.3f);
                speed = 12.0f;
                //print(StaminaBar.instance.currentStamina);
            }

            else if (StaminaBar.instance.currentStamina <= 20)
            {
                StaminaBar.instance.UseStamina(0.15f);
                speed = 4.0f;
            }
        }
        else if (StaminaBar.instance.currentStamina <= 20)
        {
            StaminaBar.instance.RegenStamina(0.02f);
            speed = 4.0f;
        }
        else
        {
            StaminaBar.instance.RegenStamina(0.05f);
            speed = 8.0f;
        }
    }
    void SlowSpeed()
    {
        SetupLocalPlayer health = GetComponent<SetupLocalPlayer>();
        if (health.healthValue <= 50)
        {
            print("slow");
            speedDown = true;
            StartCoroutine(Slower());
        }
    }
    void Slow()
    {
        if (speedDown == true)
        {
            speed = 4.0f;
        }
    }
    IEnumerator Slower()
    {
        yield return new WaitForSeconds(5);
        speedDown = false;
    }
}