using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    BoxCollider2D boxCollider;
    UIManager uiManager;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            uiManager.DialogueBoxActive();
        }
    }
}
