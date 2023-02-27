using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : MonoBehaviour
{
    public bool casting = false;
    private int[] seals;// = new int[2];

    public List<int> loadSeals = new List<int>();

    public GameObject projectile;
    public Transform castPoint;

    public float slowedTime = 0.5f;

    public bool dangerClose = false;
    public bool isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire3"))
        {
            casting = true;

            LoadSeals();



        }
        else if (Input.GetButtonUp("Fire3"))
        {
            Debug.Log("Done casting");
            casting = false;
            GenerateSpell();
        }

        if (!casting)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("punch");
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("kick");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            dangerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            dangerClose = false;
        }
    }

    protected void LoadSeals()
    {
        Debug.Log("casting");
        if (casting)//while (casting)
        {
            SlowTime();
            Debug.Log("casting = true");
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //seals[0] = 1;
                loadSeals.Add(1);
                seals = loadSeals.ToArray();
                Debug.Log("Loaded seal 1");
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                //seals[1] = 2;
                loadSeals.Add(2);
                seals = loadSeals.ToArray();
                Debug.Log("Loaded seal 2");
            }

            //casting = false;
        }

    }

    protected void GenerateSpell()
    {
        if (seals != null)
        {
            if (seals.Length < 2)
            {
                Instantiate(projectile, castPoint.position, castPoint.rotation);
                //projectile.transform.localScale *= 3;
                //Vector2 direction = new Vector2(transform.localScale.x, 0);
                //projectile.GetComponent<ThrowableWeapon>().direction = direction;
                Debug.Log("childs play");
            }
            else if (seals.Length < 4)
            {
                Instantiate(projectile, castPoint.position, castPoint.rotation);
                Instantiate(projectile, new Vector3(castPoint.position.x, castPoint.position.y + 1f), castPoint.rotation);
                Debug.Log("medium");
            }
            else if (seals.Length >= 4)
            {
                Instantiate(projectile, castPoint.position, castPoint.rotation);
                Instantiate(projectile, new Vector3(castPoint.position.x, castPoint.position.y + 1f), castPoint.rotation);
                Instantiate(projectile, new Vector3(castPoint.position.x, castPoint.position.y + 2f), castPoint.rotation);
                Debug.Log("SPICY!");
            }
            ResetTime();
        }
        else
        {
            ResetTime();
            Debug.Log("Nothing happened");
        }
        seals = null;
        loadSeals.Clear();

    }

    protected void GenerateEffect(int[] seals)
    {
        
    }

    protected void SlowTime()
    {
        if (isJumping || dangerClose)
        {
            Time.timeScale = slowedTime;
            //Time.fixedDeltaTime = slowedTime * Time.timeScale;
        }
    }

    protected void ResetTime()
    {
        Time.timeScale = 1;
        //Time.fixedDeltaTime = 1 * Time.timeScale;
    }
}
