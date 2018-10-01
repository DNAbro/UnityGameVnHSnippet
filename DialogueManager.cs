using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    XMLReader myXMLReader;                      //So it can have accesss to what the XMLReader has. Makes sense right?
    private string currentPath;                 //The current path in the XML
    private string[] pathArrays = new string[30];

    public bool dialogueActive;                 //Determines if dialogue is currently active or not.
    public GameObject dialogueBox;              //The gameObject dialouge box. It's where the text goes. Teehee
    public GameObject currentFace;
    public Sprite[] dialogueBoxSpriteArray = new Sprite[10];    
    public string displayLine;                  //The line that will currently be displayed. Not sure if good idea yet.

    public Sprite[] currentFaceDialogueArray;   //for speaking roles or whatever. isMainCharTalkingIs Needed Bool

    public string[] listOfLines;                //For loading in multiple lines
    private int currentLineNum;                 //Keeps track of lines.   
    private int faceNum;                        //Keeps track of current Face number  
    private float nextActionTime = 0.0f;   
    public bool startedReading { set; get; }
    public bool finishedReading { set; get; }               //Idea to get Super Text Mesh to play nice.
    public bool superTextMeshIsReading { get; set; }        //If it is currently reading
    public bool currentDialogueHasMoreThanOneLine { get; set; }
    public int numberOfLoadedInLines { set; get; }
    public bool thereAreNoMoreLines { set; get; }
    public bool isAMainCharTalking { set; get; }            //for changing text box and display faces
    public bool inBattleTalking { set; get; }               //for changing the in battle dialogue
    public SuperTextMesh stm;

    public static bool dialogueManagerExists;


    //Dialogue Manager needs to stay alive.
	// Use this for initialization
	void Start () {
        if (!dialogueManagerExists)                  //makes sure there aren't any doubles and persist through scenes.
        {
            dialogueManagerExists = true;
            DontDestroyOnLoad(transform.gameObject);
            //Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("I am about to destroy a DialogueManager.");
            Destroy(gameObject);
        }


        myXMLReader = GetComponent<XMLReader>();
        listOfLines = new string[30];           //Seems excessive, yet fine.
        currentLineNum = 0;
        startedReading = false;
        finishedReading = false;
        currentDialogueHasMoreThanOneLine = false;
        thereAreNoMoreLines = false;
        numberOfLoadedInLines = 0;
        isAMainCharTalking = false;
        faceNum = 0;
        inBattleTalking = false;
        closeDialogue();           //Makes sure that the dialouge box isn't active in the scene at start.
        
	}
	
	// Update is called once per frame
	void Update () {


        if (isAMainCharTalking)
        {
            stm.GetComponent<RectTransform>().anchoredPosition = new Vector3(75f,0f,0f);
            stm.GetComponent<RectTransform>().localScale = new Vector3(0.8282f,1f,1f);
            
            if (Time.time > nextActionTime)
            {
                nextActionTime += stm.readDelay + 0.1f;
                if(superTextMeshIsReading)
                    cycleThroughFaces();
                //may have to set next Action Time again idk
            }
            currentFace.GetComponent<Image>().sprite = currentFaceDialogueArray[faceNum];
            currentFace.SetActive(true);
        }

        if (inBattleTalking)
        {
            dialogueBox.GetComponent<RectTransform>().localScale = new Vector3(0.83749f, 1f, 1f);
        }
        

        superTextMeshIsReading = stm.reading;           //sets the boolean for access elsewhere.

        if (MediatorMainCharacterToTrigger.itIsOkayForDialogueToPopUpNow)       //if the dialogue got the okay to start up.
        {

            if (MediatorMainCharacterToTrigger.hasMultipleLines)        //If there is more than one line to read in.
            {
                currentDialogueHasMoreThanOneLine = true;               //Sets it to true so TalkingState can know.
                getPathArrayFromMediator();                     //Gets a path array if it needs to from the mediator.

                getLinesFromXmlReader();                        //It gets the lines from the XML reader.
                displayLine = listOfLines[currentLineNum];      //Sets the current display line from the array.
            }
            else
                displayLine = getPathThenGetLine();         //sets the current display line.
                   

            if (!stm.reading)                           //if it isn't reading in dialouge.
            {
                if (!startedReading && !finishedReading)    //if it hasn't started reading and it hasn't finished reading at one point.
                {
                    stm.Text = displayLine;                 //sets Super Text mesh as the current display line.
                    startedReading = true;                  //says it has started reading.
                }
                //line has been read in
            }
            ShowDialogue();                                    
        }       
	}

    private void ShowDialogue()
    {
        dialogueActive = true;                  //Sets the Dialogue Active to true so it can know if it's true or not.
        dialogueBox.SetActive(true);            //Turns on the Dialogue box.
    }
    public void closeDialogue()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);
        currentFace.SetActive(false);
        MediatorMainCharacterToTrigger.itIsOkayForDialogueToPopUpNow = false;
        finishedReading = false;
        startedReading = false;
        currentLineNum = 0;
        currentDialogueHasMoreThanOneLine = false;
        thereAreNoMoreLines = false;
        numberOfLoadedInLines = 0;
        whichDialogueBoxIsTheCurrentOne("");            //should reassign the normal dialogue box.
        stm.GetComponent<RectTransform>().anchoredPosition = new Vector3(-11f, 0f, 0f);
        stm.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        dialogueBox.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        inBattleTalking = false;
        faceNum = 0;

        //need to reset stm size after testing.
    }

    public void cycleThroughFaces()
    {
        if (faceNum >= currentFaceDialogueArray.Length-1)
            faceNum = 0;
        //currentFace.GetComponent<Image>().sprite = currentFaceDialogueArray[faceNum];

        faceNum++;


       
    }

    public void whichDialogueBoxIsTheCurrentOne(string type)
    {
        if (type == "main")
        {
            dialogueBox.GetComponent<Image>().sprite = dialogueBoxSpriteArray[1];  //maybe this will work and play nice
            isAMainCharTalking = true;
            
        }
        else
        {
            dialogueBox.GetComponent<Image>().sprite = dialogueBoxSpriteArray[0];
            isAMainCharTalking = false;
        }
    }

    public void thisIsbattleDialogue()
    {
        inBattleTalking = true;
    }

    public void setCurrentFaceDialogueArray(Sprite[] setMe)
    {
        currentFaceDialogueArray = setMe;
    }


    private void getPathFromMediator()
    {
        currentPath = MediatorDialogueToNPCs.stringThatWillBePassed;    //Gets the path from the Mediator that was from the NPC.
        //Debug.Log("Current Path in Dialogue Manager:" + currentPath);
    }
    private void getPathArrayFromMediator()
    {
        pathArrays = MediatorDialogueToNPCs.stringArrayThatWillBePassed;
        //Debug.Log("Current Path in pathArrays[0]:" + pathArrays[0]);
    }

    private string getLineFromXmlReader()
    {
        return myXMLReader.returnSingleLine(currentPath);               //Gets the line from the XML reader.
    }
    private void getLinesFromXmlReader()                                //For getting multiple lines from the XML reader.
    {
        int count = 0;                                                  //To properly do this.
        foreach (string path in pathArrays)
        {
            currentPath = pathArrays[count];                             //Sets the current path to the pathArrays[count]
            if (currentPath != "")                                       //Prevent it from reading in blank lines.
            {
                listOfLines[count] = getLineFromXmlReader();             //adds the line from the XML Reader to ListOfLines
                count++;
            }
            numberOfLoadedInLines = count;                              //So it can tell when to stop counting later on.
            
        }
    }
    private string getPathThenGetLine()                                 //Just calls two functions sequentially since it makes sense to.
    {
        getPathFromMediator();
        return getLineFromXmlReader();
    }

    public void advanceDialogueToEnd()                                  //advances the STM to the end.
    {
        stm.SkipToEnd();
    }

    public void readInNextLine()                                        //Reads in next line.
    {
        currentLineNum++;
        if (currentLineNum+1 == numberOfLoadedInLines)                  //Tells it not to read in one early so it doesn't try and read a blank line.
            thereAreNoMoreLines = true;
        displayLine = getPathThenGetLine();                             //the current display line will the path then line.

    }

    
}
