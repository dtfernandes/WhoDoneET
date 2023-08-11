﻿using System.Collections;
using UnityEngine;
using DialogueSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

//Class responsible for the handling of the Dialogue Display
public class DialogueDisplayHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject border;

    [SerializeField]
    private Control _control;

    /// <summary>
    /// Text component responsible for displaying the Dialogue text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI dialogueDisplayTarget = default;

    /// <summary>
    /// Current Dialogue script beeing displayed 
    /// </summary>
    [SerializeField]
    private IDialogueScript currentScript = default;

    /// <summary>
    /// Time between each char of the Dialogue
    /// </summary>
    [SerializeField]
    private float displaySpeed = default;

    /// <summary>
    /// GameObject that defines the container of the ChoiceButtons
    /// </summary>
    [SerializeField]
    private GameObject buttonLayout = default;

    /// <summary>
    /// Prefab of the ChoiceButton
    /// </summary>
    [SerializeField]
    private GameObject buttonPREFAB = default;

    /// <summary>
    /// Variable that defines the data of one line of dialogue
    /// </summary>
    private NodeData dialogueLine;

    /// <summary>
    /// Text component of the current line of dialogue
    /// </summary>
    private string dialogueText;

    /// <summary>
    /// Auxiliar vairable that represents the time between each char 
    /// in one line of dialogue
    /// </summary>
    private WaitForSeconds effectSpeed;

    /// <summary>
    /// Variable that defines if tha Dialogue has ended
    /// </summary>
    public bool Ended { get; private set; }

    /// <summary>
    /// Variable that defines if the current line of dialogue is 
    /// currenly beeing displayed
    /// </summary>
    public bool InDialogue { get; private set; }

    public System.Action<IDialogueScript> onStartDialogue;
    public System.Action<NodeData> onStartLine;
    public System.Action onEndDialogue;

    private WaitForSeconds endDelay = new WaitForSeconds(0.01f);

    //Choice being selected during the dialogue
    private int _currentChoiceIndex;
    private List<ChoiceSelector> _choices;

    private void Awake()
    {
        _choices = new List<ChoiceSelector>();
    }

    /// <summary>
    /// Method responsible for switching to the passed DialogueScript
    /// </summary>
    /// <param name="script">Dialogue Script to inicialize</param>
    public void StartDialolgue(IDialogueScript script)
    {
        border.SetActive(true);

        currentScript = script;
        onStartDialogue?.Invoke(currentScript);
        PrepareNewDialogue();
    }
    
    /// <summary>
    /// Method responsible for seting up the Dialogue
    /// </summary>
    public void PrepareNewDialogue()
    {
        //Display Specificationss
        effectSpeed = new WaitForSeconds(displaySpeed);

        //Initialize First Line
        dialogueLine = currentScript[0];
        dialogueText = currentScript[0].Dialogue;

       
        StartLine();
    }


    /// <summary>
    /// Method responsible for selecting and instantiating the respective 
    /// Choice Buttons of the current Dialogue 
    /// </summary>
    private void InstatiateChoices()
    {
        //Delete all instatiated buttons
        foreach (Transform g in buttonLayout.transform)
        {
            Destroy(g.gameObject);
        }

        _choices.Clear();

        //Get the amount of choices in the dialogue
        int choiceNumb = dialogueLine.OutPorts?.Count ?? 0;
        if (choiceNumb == 0) return;


        for (int i = 0; i < choiceNumb; i++)
        {
            if(dialogueLine.OutPorts[i].ChoiceText == "") continue;

            GameObject temp = Instantiate(buttonPREFAB, transform.position,
                Quaternion.identity, buttonLayout.transform);


            ChoiceSelector cs = temp.GetComponent<ChoiceSelector>();
            cs.ChangeChoiceText(dialogueLine.OutPorts[i].ChoiceText);
            cs.ChoiceNumb = i;
            cs.NextLine = NextLine;
            _choices.Add(cs);
        }

        if (_control == Control.FullKeyboard)
            _choices[0].SetHighlight(true);

        buttonLayout.SetActive(false);
      
    }



    public void StartLine()
    {
        if (dialogueLine.CustomFunctions != null)
        {
            //Play Custom Events
            foreach (CustomFunction func in dialogueLine.CustomFunctions)
            {
                func.Invoke();
            }
        }

        //Handle button Layout
        InstatiateChoices();
        DisplayLine();
    }

    /// <summary>
    /// Method responsible for deciding initializing next line of the current 
    /// DialogueScript
    /// </summary>
    /// <param name="choice">The selected choice of the current line</param>
    public void NextLine(int choice)
    {
        dialogueLine =
               currentScript.GetNextNode(dialogueLine, choice);

        if (dialogueLine == null)
        {
            EndDialogue();
            return;
        }

        dialogueText = dialogueLine.Dialogue;

        StartLine();
    }


    /// <summary>
    /// Method responsible for ending the current DialogueScript
    /// </summary>
    private void EndDialogue()
    {
        StartCoroutine("EndDialogueDelay");
        border.SetActive(false);     
        dialogueDisplayTarget.text = "";
        StopCoroutine("TypeWriterEffect");
    }


    /// <summary>
    /// Method that starts the next line in the Dialogue
    /// </summary>
    private void DisplayLine()
    {
        onStartLine?.Invoke(dialogueLine);
        StopCoroutine("TypeWriterEffect");
        StartCoroutine("TypeWriterEffect");
    }


    /// <summary>
    /// IEnumerator that creates a TypeWriteEffect
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeWriterEffect()
    {
        //Wait a bit so inputs don't overlap
        yield return endDelay;

        InDialogue = true;
        Ended = false;
        
        dialogueDisplayTarget.text = "";
        int index = 0;
        while (dialogueText.Length > 0)
        {
            yield return effectSpeed;
            
            //Feio
            index++;
            //Very feio
            if (dialogueLine.Events != null)
            {
                foreach (EventTriggerData data in dialogueLine.Events)
                {
                    if (data.IndexPos == index)
                    {

                        GameObject obj = DialogueEventManager.GetGameObject(data.UniqueID);
                        MonoBehaviour mono = obj.GetComponent(data.SelectedComponent) as MonoBehaviour;
                        mono.Invoke(data.FunctionName, 0);

                        //DialogueUniqueId[] test = GameObject.FindObjectsOfType<DialogueUniqueId>();
                        //GameObject currentObject = null;

                        //foreach(DialogueUniqueId d in test)
                        //{
                        //    if(d.UniqueID == data.UniqueID)
                        //    {
                        //        currentObject = d.gameObject;

                        //        MonoBehaviour obj = currentObject.GetComponent(data.SelectedComponent) as MonoBehaviour;
                        //        obj.Invoke(data.FunctionName,0);
                        //        break;
                        //    }

                        //}

                    }
                }
            }

            char nextChar = dialogueText[0];
            dialogueDisplayTarget.text += nextChar;
            dialogueText = dialogueText.Substring(1);
        }
        Ended = true;
        buttonLayout.SetActive(true);
    }

    IEnumerator EndDialogueDelay()
    {
        yield return endDelay;

        //Call all custom events
        onEndDialogue?.Invoke();
        InDialogue = false;
    }

    void OnMove(InputValue value)
    {
        if (!InDialogue) return;
        if (_control != Control.FullKeyboard) return;


        _choices[_currentChoiceIndex].SetHighlight(false);

        Vector2 inputVector = value.Get<Vector2>();
        float moveInput = inputVector.y;


        int listLenght = _choices.Count;

        //Select option
        if (moveInput > 0) // Move to the next index
        {
            _currentChoiceIndex = (_currentChoiceIndex + 1) % listLenght;
        }
        else if (moveInput < 0) // Move to the previous index
        {
            _currentChoiceIndex = (_currentChoiceIndex - 1 + listLenght) % listLenght;
        }


        _choices[_currentChoiceIndex].SetHighlight(true);
    }

    void OnInteract()
    {
        if(GameSettings.Instance.isMenuOpen) return;
        if (!InDialogue) return;

        if (Ended)
        {
            if ((dialogueLine.OutPorts?.Count ?? 0) > 0)
            {
                if (dialogueLine.OutPorts[0].ChoiceText != "") return;
            }

            NextLine(0);
        }
        else
        {
            dialogueDisplayTarget.text += dialogueText;
            dialogueText = "";
            buttonLayout.SetActive(true);
            Ended = true;
            StopCoroutine("TypeWriterEffect");
        }
    }

}
