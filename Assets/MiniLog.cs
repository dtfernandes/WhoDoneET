using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniLog : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _anim.SetBool("Selected", true);
        _anim.SetTrigger("Select");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _anim.SetBool("Selected", false);
        _anim.SetTrigger("Select");
    }
}
