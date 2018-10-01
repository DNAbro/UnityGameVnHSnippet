using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingState : CharStateAbstract {

    bool battleIsDoneTalking = false;

    //In this state, all it needs to do is advance the dialogue and make choices. Can't move.
    public override void run()
    {
        Debug.Log("In talking state.");
        advanceDialogue();
    }



    void advanceDialogue()
    {
        //this will either advance the dialogue to the end of the sentence, move to next dialogue or close it.
        if(Input.GetKeyDown("z"))
        {
            Debug.Log("Key was pressed down in TalkingState.");
            bool isReading = GameObject.Find("DialogueManager").GetComponent<DialogueManager>().superTextMeshIsReading;
            if (isReading)
                GameObject.Find("DialogueManager").GetComponent<DialogueManager>().advanceDialogueToEnd();  //Tells Dialogue Manager to advance dialoge to End.
            else if(GameObject.Find("DialogueManager").GetComponent<DialogueManager>().currentDialogueHasMoreThanOneLine && !isReading && !GameObject.Find("DialogueManager").GetComponent<DialogueManager>().thereAreNoMoreLines){
               
                GameObject.Find("DialogueManager").GetComponent<DialogueManager>().startedReading = false;  //It is setting this so it can display the next line properly.
                GameObject.Find("DialogueManager").GetComponent<DialogueManager>().readInNextLine();        //Tells it to read it the next line.
            }
            else //if there are no more lines.
            {
                GameObject.Find("DialogueManager").GetComponent<DialogueManager>().closeDialogue();         //Closes the dialogue if there are no more lines.
                if (GetComponent<MainCharacter>().inBattle)                                                 //just added
                {
                    GetComponent<MainCharacter>().switchToBattleState();
                    battleIsDoneTalking = true;                         //to let enemy script know it can move off of talking phase.
                }
                else
                {
                    FindObjectOfType<CutsceneManager>().movingBackFromTalkState();              //Switches back to not talking in cutscene if it is in one.
                    GetComponent<MainCharacter>().switchToOverworldState();                                     //Switches back to the overworld state, for now.
                }
            }
        }
    }

    public bool getBattleIsDoneTalking()
    {
        return battleIsDoneTalking;
    }
    public void setBattleIsDoneTalking(bool a) { battleIsDoneTalking = a; }

    //There should be a a dialogue choice thingy.
}
