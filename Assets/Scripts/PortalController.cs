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
    private bool isMovingInPortal = false;

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
        if (!Interacted && !isMovingInPortal)
        {
            StartCoroutine(Interact());
            Debug.Log("Portal Interaction Activated (Portal Controller)");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   
        GameObject other = collision.gameObject;
        PlayerController playerController = other.GetComponent<PlayerController>();
        playerController.portalController = this;

        if (collision.CompareTag("Player") && Interacted && !isMovingInPortal)
        {
            StartCoroutine(PortalEnter());
            Debug.Log("Portal Enter Activated (Stay Trigger)");
        }
        else Debug.Log("Portal Enter Not Activated (Stay Trigger)");
    }

    IEnumerator Interact()
    {   
        Interacted = true;
        yield return new WaitForSeconds(0.1f);
        Interacted = false;
    }

    IEnumerator PortalEnter()
    {
        if (!isMovingInPortal)
        {
            isMovingInPortal = true;
            rb.simulated = false;
            anim.SetTrigger(AnimStrings.EnterPortal);
            yield return StartCoroutine(MoveInPortal());
            player.transform.position = destination.position;
            anim.SetTrigger(AnimStrings.ExitPortal);
            yield return new WaitForSeconds(0.5f);
            rb.simulated = true;
            isMovingInPortal = false;
        }
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
