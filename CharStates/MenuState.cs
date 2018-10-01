using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : CharStateAbstract {

    public Menu menu;                         //in Unity Editor, this is probably bad.
    public override void run()
    {
        Debug.Log("In menu state.");
        getNameAndHealthAndPassToMenu();
        getItemListAndDisplay();
        menu.setMenuActive();

        chooseFromMain();
        moveChoiceInMainList();
        backOut();

        //getNameAndHealthAndPassToMenu();    //testing
        //getItemListAndDisplay();
    }

    void increaseCount(){                   //increases the count in the Menu class. This is for choosing Items, Powers, Calls, Info, Quit.
        if(menu.choiceCount < 4)
            menu.choiceCount++;
        menu.tempStop = true;               //makes the action happen once in Menu.
    }           
    void decreaseCount(){
        if(menu.choiceCount > 0)
            menu.choiceCount--;
        menu.tempStop = true;
    }

    void chooseFromMain()
    {
        if (Input.GetKeyDown("z"))
            menu.chooseFromList();
    }

    void backOut()                      //should backout either closing the menu or backing out to main choices
    {
        if (Input.GetKeyDown("x"))
        {
            if(!menu.hasMadeChoiceAndIsNowInSubMenu)            //to back out from menu.
                GetComponent<MainCharacter>().switchToOverworldState();
            menu.backOrClose();
            


        }
    }

    void moveChoiceInMainList()
    {
        if (!menu.hasMadeChoiceAndIsNowInSubMenu)
        {
            if (Input.GetKeyDown("down"))
                increaseCount();
            if (Input.GetKeyUp("up"))
                decreaseCount();
        }
        
    }

    void getNameAndHealthAndPassToMenu()
    {
        int h,curr,money;
        string name, hname;

        h = FindObjectOfType<MainCharacter>().health;
        curr = FindObjectOfType<MainCharacter>().currentHealth;
        name = FindObjectOfType<MainCharacter>().playerName;
        hname = FindObjectOfType<MainCharacter>().heroName;
        money = FindObjectOfType<MainCharacter>().money;

        //Note I had a WEIRD bug that caused the text to stay on screen after being disabled when using .Text rather than .text.
        menu.health.text = "HP: " + h + "/" + curr;
        menu.name.text = name + "(" + hname + ")";
        menu.money.text = "Money:" + money + "g";
        Debug.Log("Health:" + h);
        Debug.Log("Name:" + name);
        Debug.Log("Hero Name:" + hname);
    }

    void getItemListAndDisplay()
    {
        foreach( ItemOnPerson a in MainCharacter.itemBag)
        {
            Debug.Log(a.name);
            menu.sub1.text = a.name;
        }
        //note this should only be on once in correct submenus
    }
}
