using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movement : MonoBehaviour
{
    public int speed=100;
    public float speed2 = 10f;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject attackpoint;
    bool isranged = true;
    public GameObject gun;
    public Vector3 shootdirection;
    public float LARPMACHINE;

    [SerializeField]
    private bool animated = false;


    // Start is called before the first frame update

    private void Start()
    {
        
        //attackpoint = gameObject.transform.Find("atk").gameObject;
        //gun = attackpoint.transform.Find("gun").gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
        
         
         //GameObject.Find("AstarPath").GetComponent<AIqueue>().players.Add(gameObject);         
        
    }
    // Update is called once per frame
    void Update()
    {
       
            
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");


        if (animated)
        {
            animator.SetFloat("Speed", Mathf.Abs(inputY) + Mathf.Abs(inputX));
            if (Mathf.Abs(inputY) + Mathf.Abs(inputX) > 0.01f)
            {
                animator.SetFloat("LastHorizontal", inputX);
                animator.SetFloat("LastVertical", inputY);
                animator.SetFloat("Horizontal", inputX);
                animator.SetFloat("Vertical", inputY);
            }
        }
     
           

        if (!gameObject.GetComponent<playerStats>().busy)
        {
            float x = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(inputX * speed2 , inputY  *speed2);
        }

        if (inputX != 0 && !isranged || inputY != 0 && !isranged)
        attackpoint.transform.position = new Vector2(gameObject.transform.position.x + Sign(inputX), gameObject.transform.position.y + Sign(inputY));
        if (isranged)
        {
            
            Vector3 mousePosition = transform.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            shootdirection = (mousePosition - gameObject.transform.position).normalized;
            if (transform.position.x - mousePosition.x < 0)
            {

                gun.GetComponent<SpriteRenderer>().flipY = false;
            }
            else
            {
              
               
                gun.GetComponent<SpriteRenderer>().flipY = true;
            }
            float angle = Mathf.Atan2(shootdirection.x, shootdirection.y) * Mathf.Rad2Deg;
            LARPMACHINE = Mathf.Lerp(LARPMACHINE, 0, Time.deltaTime*6);
           
            attackpoint.transform.eulerAngles = new Vector3(0, 0, 90 - angle + LARPMACHINE);
           
             
        }

        //rb.AddForce(Player.transform.TransformVector(new Vector2(inputX, inputY)) * 2.0f); //snoww!!!
    }
    
        static int Sign(float number)
        {
            if (number < 0)
            {
                return -1;
            }
            else if (number > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
}


    //Unity thinks 0 is a positive number so I had to write my own function
   

    



