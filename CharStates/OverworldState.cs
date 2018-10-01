using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldState : CharStateAbstract {

    //Overworld State Movement, swapt in and out.
    public float overworldSpeed = 4;
    public bool isInPositionToPickUpItem { get; set; }      //self explanatory, is made true/false

    public ItemAbstract tempItem { get; set; }              //in order to have temporary access to the item when standing on it. So you can do stuff with it.
    //probably bad encumberence but I don't give a fuck. ItemAbstract is where it is set.
    
	
    override public void run()
    {
        Debug.Log("Movement in Overworld.");

        if (!FindObjectOfType<CutsceneManager>().cutSceneIsActive)
        {
            movement();
            interact();
            bringUpMenu();
        }
        else          //stops animation
        {
            setBoolIsMovingToFalse();
        }
    }

    void bringUpMenu()
    {
        if (Input.GetKeyDown("x"))
            GetComponent<MainCharacter>().switchToMenuState();
    }

    void interact()
    {
        //TESTING, WON'T BE USED IN REALITY
        if (Input.GetKeyDown("q"))
        {
            GetComponent<MainCharacter>().switchToBattleState();
            FindObjectOfType<BattleManager>().TestOutSimpleBattleTransition();
            
        }

        if (Input.GetKeyDown("a"))
        {
            //FindObjectOfType<CurrentGameManager>().saveFlagsAndCreateSave();
        }
        if (Input.GetKeyDown("s"))
        {
            FindObjectOfType<CurrentGameManager>().loadFlagsFromSave();
        }

        //Testing

        if(MediatorMainCharacterToTrigger.mainCharacterIsInTriggerZone)     //Grabs info from mediator.
            if (Input.GetKeyDown("z"))
            {
                Debug.Log("Z was pressed.");
                MediatorMainCharacterToTrigger.mainCharacterHasPressedZbool = true;

                //testing
                //Finds the gameObject for the mediator and creates a personal one to reference.
                MediatorMainCharacterToTrigger mmctt = GameObject.Find("TriggerToMCMediator").GetComponent<MediatorMainCharacterToTrigger>();
                mmctt.mainCharacterHasPressedZ();       //message to send to Mediator.


                GetComponent<MainCharacter>().switchToTalkingState();           //Swtiches the state to talking State.
            }

        if(isInPositionToPickUpItem)                    //when in a collider of item, it should be set to true. When move out, false it.
        {
            if (Input.GetKeyDown("z"))
            {
                Debug.Log("Z was pressed in Item zone.");
                Debug.Log(tempItem);
                tempItem.pickUp();          //tells the item that was passed referenced to be picked up. 

            }
        }
    }
    void movement()
    {
        //debugAnim();

        
        if(Input.anyKeyDown != true)    //this is hacky, fix afterwards
        {
            setBoolIsMovingToFalse();
        }

        if (Input.GetKey("right"))
        {
            //Animation
            setBoolIsMovingToTrue();
            setAnimInputXToNeg1();
            setAnimInputYTo0();
            //Movement
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector2.right * Time.deltaTime * overworldSpeed);
        }
        else if (Input.GetKey("left"))
        {
            //Animation
            setBoolIsMovingToTrue();
            setAnimInputXToNeg1();
            setAnimInputYTo0();
            //Movement
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector2.left * Time.deltaTime * -overworldSpeed);
        }

        if (Input.GetKey("up"))
        {
            //Animation
            setBoolIsMovingToTrue();
            setAnimInputYTo1();
            //Movement
            transform.Translate(Vector2.up * Time.deltaTime * overworldSpeed);
        }
        else if (Input.GetKey("down"))
        {
            //Animation
            setBoolIsMovingToTrue();
            setAnimInputYToNeg1();
            setAnimInputXTo0();
            //Movement
            transform.Translate(Vector2.down * Time.deltaTime * overworldSpeed);
        }

        //float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        //transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    
}
