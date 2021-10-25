using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public AudioClip backgroundMusic; //BGM audio created
    public AudioClip coinGet; //coin audio created
    public AudioClip fanfare; //fanfare audio created
    public AudioSource musicSource; // music source created
    public AudioSource sfxSource; // sfx source created
    public GameObject winTextObject; // Win text created
    public GameObject loseTextObject; // lose text created
    
    private Rigidbody2D rd2d; //rigid body is created
    
    public float speed; //speed value is created

    private SpriteRenderer sprite;

    public Text score; //score text created
    public Text countLives; // lives UI created
    
    private int scoreValue = 0; // score & value are created
    private int lives = 3; //life & value are created

    Animator anim; //anim variable for animator

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>(); //r2d2 is made the rigid body
        score.text = "Coins: " + scoreValue.ToString(); //default score UI
        countLives.text = "Lives: " + lives.ToString(); //default life UIe

        musicSource.clip = backgroundMusic; //bgm selected
        musicSource.Play(); //bgm playing
        musicSource.loop = true; //will loop

        anim = GetComponent<Animator>(); // anim gets the animator

        SetCountText(); //look at setcounttext
        winTextObject.SetActive(false); //default win text state
        
        SetCountLives(); //look at setcountlives
        loseTextObject.SetActive(false); //default lose text state
    }

    void SetCountLives()
    {
        if(lives == 0) 
        {
            loseTextObject.SetActive(true);
            winTextObject.SetActive(false);
            transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            Destroy(this.gameObject);
        }
    }

    void SetCountText()
    {
        if(scoreValue == 4)
        {
            transform.position = new Vector3(50.0f, 13.0f, 0.0f); //X-Axis, Y-axis, Z-axis
            lives = 3; //reset life
            countLives.text = "Lives: " + lives.ToString(); //updated life UI
        }

        else if(scoreValue == 8)
        {
            winTextObject.SetActive(true);
            musicSource.clip = fanfare; //fanfare song selected
            musicSource.Play(); //play fanfare
            musicSource.loop = false; //don't loop
        }
    }

    // FixedUpdate is called for physics
    void FixedUpdate() //movement enabled
    {
        float hozMovement = Input.GetAxis("Horizontal"); //making hozMovement be the a and d keys
        float verMovement = Input.GetAxis("Vertical"); //making vertical movement be the w and s keys

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
    }

    private void OnCollisionEnter2D(Collision2D collision) //collissions enabled
    {
        if(collision.collider.tag == "Coin") //coin tag brought in
        {
            scoreValue +=1; //coin value manipulation
            score.text = "Coins: " + scoreValue.ToString(); //updated score UI
            Destroy(collision.collider.gameObject); //coin destruction
            sfxSource.clip = coinGet; //bring the sound file
            sfxSource.Play(); //play it
            sfxSource.loop = false; //don't loop
            SetCountText();
        }

        if(collision.collider.tag == "Enemy") //enemy tag brought in
        {
            lives -=1; //life value manipulation
            countLives.text = "Lives: " + lives.ToString(); //updated life UI
            Destroy(collision.collider.gameObject);//destroys enemy
            SetCountLives(); //look at set count lives
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground") //ground tag brought in
        {
            if(Input.GetKey(KeyCode.W)) //jump key
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //jump force
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey("escape")) // quit game
        {
          Application.Quit();
        }
        
        Vector3 characterScale = transform.localScale;
        if (Input.GetAxis("Vertical") > 0) //if youre going horizontal
        {
            characterScale.x = .75f; //scale character
            anim.SetInteger("State", 2); //fall animation
        }
        else if (Input.GetAxis("Horizontal") < 0) //if youre going horizontal
        {
            characterScale.x = -.75f; //scale character
            anim.SetInteger("State", 1); //run animation
        }
        else if (Input.GetAxis("Horizontal") > 0) //if youre going horizontal
        {
            characterScale.x = .75f; //scale character
            anim.SetInteger("State", 1); //run animation
        }
        else if (Input.GetAxis("Vertical") < 0) //if youre going Vertical
        {
            characterScale.x = .75f; //scale character
            anim.SetInteger("State", 2); //fall animation
        }
        else
        {
            anim.SetInteger("State", 0); //if nothing, idle animation
        }

        transform.localScale = characterScale;
    }

}
