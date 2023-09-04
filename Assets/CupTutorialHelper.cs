using UnityEngine;

public class CupTutorialHelper : MonoBehaviour
{
    private PickupableObject _cup;

    [SerializeField]
    private GameObject sillyTutString;

    [SerializeField]
    private Interactor _interactor;

    [SerializeField]
    private TutorialManager _manager;


    private int tutorialInt; 

    void Awake()
    {
        _cup = GetComponent<PickupableObject>();

        _cup.onGrab += () => 
        {
            if (tutorialInt == 0)
            {
                tutorialInt = 1;
                sillyTutString.SetActive(true);
            }
        };

        _interactor.onInteract += (t) =>
        {
            if(t == InteractionType.Dialogue){
                if(tutorialInt == 1)
                {
                    tutorialInt = 2;
                    sillyTutString.SetActive(false);
                }
            }

        };


        _cup.onDrop += () =>
        {
            if (tutorialInt == 2)
            {
                tutorialInt = 3;
                _manager.CupEnd();
            }
        };
    }
}
