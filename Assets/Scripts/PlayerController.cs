﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float speed;
    public float RotateSpeed;
    public int health;
    private float timeOfLastShot;
    public float shootCooldown;
    public GameObject bullet;
    private float bulletSpawnOffset;
    public GameObject game;
    private bool hasShield;
    private SpriteRenderer spriteRenderer;
    private int difficulty;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        timeOfLastShot = Time.time - shootCooldown;
        bulletSpawnOffset = 1;
        hasShield = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        difficulty = 1;
    }

    // Update is called once per frame

    void Update()
    {
        // Rotate
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, 0, 1) * -RotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, 0, 1) * RotateSpeed * Time.deltaTime);

        // Shoot
        if (Input.GetKey(KeyCode.Space) && timeOfLastShot + shootCooldown <= Time.time)
        {
            GameObject shotBullet = Instantiate(bullet, transform.position + transform.forward*bulletSpawnOffset, transform.rotation);
            shotBullet.GetComponent<BulletController>().setParent(gameObject);
            timeOfLastShot = Time.time;
        }

        checkIfOffscreen();
    }

    // Update is called once per frame right before physics happen
    void FixedUpdate () {
        //Store the current vertical input in the float moveVertical.
        float move = Input.GetAxis("Vertical");

        rb2d.velocity = new Vector2(transform.up.x, transform.up.y) * speed * move;
    }

    void checkIfOffscreen()
    {
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (transform.position.x > stageDimensions.x)
        {
            transform.position = new Vector3(-stageDimensions.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -stageDimensions.x)
        {
            transform.position = new Vector3(stageDimensions.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y > stageDimensions.y)
        {
            transform.position = new Vector3(transform.position.x, -stageDimensions.y, transform.position.z);
        }
        else if (transform.position.y < -stageDimensions.y)
        {
            transform.position = new Vector3(transform.position.x, stageDimensions.y, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Bullet" && (other.GetComponent<BulletController> ().getParent () != gameObject || Time.time > other.gameObject.GetComponent<BulletController> ().getTimeOfSpawn () + 0.1f)) 
		{
			if (hasShield) 
			{
				hasShield = false;
				transform.Find ("Shield").GetComponent<SpriteRenderer> ().enabled = false;
			} 
			else 
			{
				spawnOrb(other.GetComponent<BulletController> ().getParent().gameObject);
			}
		} 
		else if (other.tag == "ShieldProjectile") 
		{
			hasShield = true;
			transform.Find ("Shield").GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, .75f);
			transform.Find ("Shield").GetComponent<SpriteRenderer> ().enabled = true;
		} 
	}

    public void spawnOrb(GameObject bulletOwner)
    {
        string path =  "Prefabs/";

        int damage;

        if (bulletOwner.tag == "Enemy")
        {
            damage = bulletOwner.GetComponent<EnemyController>().getStrength();
        }
        else
        {
            damage = difficulty;
        }

        if (damage == 1)
        {
            path += "PinkOrb";
        }
        else if (damage == 2)
        {
            path += "GreenOrb";
        }
        else if (damage == 3)
        {
            path += "YellowOrb";
        }
        else
        {
            path += "RedOrb";
        }

        GameObject spawnedOrb = Instantiate(Resources.Load<GameObject> (path), transform.position, Quaternion.identity);

        if (health - spawnedOrb.GetComponent<OrbController>().value <= 0)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            health -= spawnedOrb.GetComponent<OrbController>().value;
            game.GetComponent<GameController>().orbs.Add(spawnedOrb);
        }   
    }

    public int getDifficulty()
    {
        return difficulty;
    }

    public void setDifficulty(int difficulty)
    {
        if (difficulty == 2)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/PlayerBlue");
            this.difficulty = 2;
        }
        else if (difficulty == 3)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/PlayerGreen");
            this.difficulty = 3;
        }
        else if (difficulty == 4)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/PlayerRed");
            this.difficulty = 4;
        }
    }
}
