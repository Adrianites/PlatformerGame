using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PortalController : MonoBehaviour
{

    [SerializeField] private Transform destination;
    GameObject player;
    Animator anim;
    Rigidbody2D rb;
    bool Interacted = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform destination)
    {
        this.destination = destination;
    }

    public void OnInteraction()
    {
        
        StartCoroutine(Interact());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Interacted == true)
        {
            StartCoroutine(PortalEnter());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Interacted == true)
        {
            StartCoroutine(PortalEnter());
        }
    }

    IEnumerator Interact()
    {   
        Interacted = true;
        yield return new WaitForSeconds(0.01f);
        Interacted = false;
    }

    IEnumerator PortalEnter()
    {
            rb.simulated = false;
            anim.SetTrigger(AnimStrings.EnterPortal);
            StartCoroutine(MoveInPortal());
            yield return new WaitForSeconds(0.5f);
            player.transform.position = destination.position;
            anim.SetTrigger(AnimStrings.ExitPortal);
            yield return new WaitForSeconds(0.5f);
            rb.simulated = true;
    }

    IEnumerator MoveInPortal()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            Debug.Log("Moving in portal");
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 4 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
    }
}
