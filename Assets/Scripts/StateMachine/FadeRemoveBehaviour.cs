using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveNehaviour : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    private float timeElapsed = 0;
    SpriteRenderer sr;
    GameObject objToRemove;
    Color startColour;


    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0;
        sr = animator.gameObject.GetComponent<SpriteRenderer>();
        startColour = sr.color;
        objToRemove = animator.gameObject;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;

        float newAlpha = startColour.a * 1 - (timeElapsed / fadeTime);

        sr.color = new Color(startColour.r, startColour.g, startColour.b, newAlpha);

        if (timeElapsed >= fadeTime)
        {
            Destroy(objToRemove);
        }
    }
}
