using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hung_Movement : MonoBehaviour
{
    public string sceneName;
    public string loadScene;
    public float moveSpeed;
    private float moveSpeedMod;
    public GameManager gameManager;
    public GameObject enemyDeathParticle;

    public float dashSpeed;
    private float dashSpeedMod;

    public float dashTime;
    private bool dashing = false;

    public float dashCooldown;
    private bool dashOnCooldown = false;

    //public float bobStrength;
    //public float bobRate;
    //private float yMove;

    private Vector3 moving;
    public CharacterController controller;
    private float horiz;
    private float vert;
    private bool isMoving;

    private float moveGravity;
    public float gravity;

    public AudioClip footsteps;
    public AudioClip swoosh;
    public AudioClip grunt;

    public AudioSource audioSource;

    //public Rigidbody selfRB;

    // Use this for initialization
    void Start()
    {
        dashSpeedMod = dashSpeed;
        //selfRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;

    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }

    IEnumerator Dash()
    {
        moveSpeedMod = moveSpeed * dashSpeedMod;
        StartCoroutine(DashTimer());
        if (Input.GetAxis("Horizontal") < 0)
        {
            horiz = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            horiz = 1;
        }
        else
        {
            horiz = 0;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            vert = -1;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            vert = 1;
        }
        else
        {
            vert = 0;
        }

        if (horiz != 0 || vert != 0)
        {
            dashing = true;
            dashOnCooldown = true;
            StartCoroutine(Cooldown());
            while (dashing)
            {
                moving = new Vector3(horiz * moveSpeedMod, 0f, vert * moveSpeedMod);
                //moveSpeedMod /= 1.2f;
                yield return null;
            }
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isMoving)
        //{
        //    audioSource.OneShot(footsteps, 0.7f);
        //}

        moveGravity = -gravity * Time.deltaTime * Time.deltaTime;
        if (Input.GetButtonDown("Dash") && !dashOnCooldown)
        {
            StartCoroutine("Dash");
        }
        else if (!dashing && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            isMoving = true;

            //yMove = Mathf.Sin(Time.time * bobRate) * bobStrength;
            moving = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0f, Input.GetAxis("Vertical") * moveSpeed);
        }
        else if (!dashing)
        {
            moving = Vector3.zero;
        }

        //print(moving.y);
        //moving.y -= moveGravity;
        //print(moving.y);
        moving.y -= gravity * Time.deltaTime;
        controller.Move(moving * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "Enemy")
        {
            gameManager.RestartGame();      
            //StartCoroutine("WaitScene");
            dashOnCooldown = false;
            dashing = false;
        }
        else if (other.gameObject.tag == "Target")
        {
            KillEnemey(other.transform.gameObject);
            StartCoroutine("Next");
        }
    }

    public IEnumerator WaitScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(loadScene);
    }

    public IEnumerator Next()
    {
        GameObject.FindGameObjectWithTag("Target").GetComponent<sceneChange>().newScene(sceneName);
        yield return new WaitForSeconds(5f);
    }

    private void KillEnemey(GameObject enemy)
    {
        Instantiate(enemyDeathParticle, enemy.transform.position, enemy.transform.rotation);
        Destroy(enemy);
    }
}
