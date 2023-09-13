using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FadeInScreen : MonoBehaviour
{
    [SerializeField]
    UnityEvent onFadeOut;
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetNotActive()
    {
        gameObject.SetActive(false);
    }

    public void StartFadeOut()
    {
        _anim.Play("FadeOut");
    }

    public void FadeOutAction()
    {
        onFadeOut?.Invoke();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
