using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator animator;

    public GameObject ConditionObj;

    public bool Condition = false;

    public string sceneToLoad;

    public void Update()
    {
        if (ConditionObj == null)
        {
            Condition = true;
        }
        else if (ConditionObj.CompareTag("Enemigo"))
        {
            Controller_Enemigo Enemigo = ConditionObj.GetComponent<Controller_Enemigo>();

            if (Enemigo.Vida <= 0) 
            {
                Condition = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Condition)
        {
            animator.SetTrigger("FadeOut");
            StartCoroutine(OnFadeComplete());
        }
    }

    private IEnumerator OnFadeComplete()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
