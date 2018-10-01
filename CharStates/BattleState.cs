using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : CharStateAbstract {

    private bool menuBattleSwitch;
    private float movementSpeed = 4;
    private int numberOfShots;      // 5 shots per turn.
    private bool powerUsed;         //if the power has been used on that turn true;
    private int countForMenu = 1;       //determines when it created or whatever.

    public BaseBullet bulletToBeSpawned;    //basic shot

    //maybe don't put it here?
    public AudioClip menuSound;
    public AudioSource soundSource;

    

    //public BaseBullet powerShot;      //Power based bullet
    
    

    //random thought but I will have to enable a pause menu regardless anywhere.
    public override void run()
    {
        //GetComponent<BaseBattle>().setMenuPhase(false); example how I do this

        //menuBattleSwitch    set to whatever the current battle is in the scene. 

        Debug.Log("In battle state.");
        
        if (FindObjectOfType<BaseBattle>().getMenuPhase())               //depending on if they are doing the menu things or battle things, control what happens.
            doMenuStuff();
        if (FindObjectOfType<BaseBattle>().getBattlePhase() || FindObjectOfType<BaseBattle>().getJusticeModePhase())
            doBattleStuff();
        if (FindObjectOfType<BaseBattle>().getTalkPhase())
            doTalkStuff();

    }

    public void doTalkStuff(){

        //the phase in BaseBattle is Talking now. Figure out how to communicate correctly.
        //Let's see if this works.
        FindObjectOfType<MainCharacter>().switchToTalkingState();
    }

    public void doMenuStuff()
    {
        Debug.Log("Doing Menu stuff");
        refill();

        //For the main menu portion. Figure out the rest later hater.
        switch (countForMenu)
        {
            case 0:
                FindObjectOfType<BattleUI>().showCurrentChoice("");
                break;
            case 1:
                FindObjectOfType<BattleUI>().showCurrentChoice("fight");
                break;
            case 2:
                FindObjectOfType<BattleUI>().showCurrentChoice("item");
                break;
            case 3:
                FindObjectOfType<BattleUI>().showCurrentChoice("power");
                break;
        }

        if (Input.GetKeyDown("down"))
        {
            if (countForMenu < 3)
            {
                soundSource.Play();
                countForMenu++;
            }
        }
        else if (Input.GetKeyDown("up"))
        {
            if (countForMenu > 1)
            {
                soundSource.Play();
                countForMenu--;
            }
        }

        Debug.Log("Count for menu is:" + countForMenu);
        
        
       
        

        //temporary for now.
        if(Input.GetKeyDown("z") && countForMenu == 1)       //switches it from the menu phase to talk phase.
        {
            FindObjectOfType<BaseBattle>().setMenuPhase(false);
            FindObjectOfType<BaseBattle>().setTalkPhase(true);
            FindObjectOfType<BattleUI>().showCurrentChoice("");         //resets menu
        }
        else if (Input.GetKeyDown("z") && countForMenu == 2)
        {
            Debug.Log("Bring up item menu.");
        }
        else if (Input.GetKeyDown("z") && countForMenu == 3)
        {
            Debug.Log("Bring up power menu.");
        }
    }

    public void doBattleStuff()
    {
        Debug.Log("Doing Battle stuff");
        movement();
        shoot();
    }

    void shoot()            //standard bullets and power
    {
        //Need to create other conditionals for Justice Mode since it goes from there.
        if (Input.GetKeyDown("z") && numberOfShots != 0)
        {
            //do shot
            numberOfShots--;
            Transform posAtTime = this.transform;
            Instantiate(bulletToBeSpawned,new Vector3(posAtTime.position.x,posAtTime.position.y,0), Quaternion.identity);         //instantiates the baseBullet
            Debug.Log("Shot used, number of shots left: " + numberOfShots);

            if (FindObjectOfType<BaseBattle>().getJusticeModePhase())
                numberOfShots = 5;
        }

        if(Input.GetKeyDown("x") && !powerUsed)
        {
            powerUsed = true;
            Debug.Log("Power used up.");
        }
    }

    void refill()           //reset the variables every menu turn.
    {
        numberOfShots = 5;
        powerUsed = false;
        Debug.Log("Shots refilled and power not used.");
    }

    void movement()
    {
        //debugAnim();


        /*if (Input.anyKeyDown != true)    //this is hacky, fix afterwards
        {
            setBoolIsMovingToFalse();
        }*/

        if (Input.GetKey("right"))
        {
            //Animation
            //setBoolIsMovingToTrue();
            //setAnimInputXToNeg1();
            //setAnimInputYTo0();
            //Movement
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);
        }
        else if (Input.GetKey("left"))
        {
            //Animation
            //setBoolIsMovingToTrue();
            //setAnimInputXToNeg1();
            //setAnimInputYTo0();
            //Movement
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector2.left * Time.deltaTime * -movementSpeed);
        }

        if (Input.GetKey("up"))
        {
            //Animation
            //setBoolIsMovingToTrue();
            //setAnimInputYTo1();
            //Movement
            transform.Translate(Vector2.up * Time.deltaTime * movementSpeed);
        }
        else if (Input.GetKey("down"))
        {
            //Animation
            //setBoolIsMovingToTrue();
            //setAnimInputYToNeg1();
            //setAnimInputXTo0();
            //Movement
            transform.Translate(Vector2.down * Time.deltaTime * movementSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyBulletDP")
        {
            //Bullets will need a damage value a bit later so make sure this is known.
            //will be something more along the lines of "col.gameObject.getDamage();
            Debug.Log("Player: I've been hit owie. ");
            FindObjectOfType<MainCharacter>().lowerHealth(2);

            //need to reduce player health
        }
    }
}
