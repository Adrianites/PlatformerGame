using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    // Pixels per second
    public Vector3 moveSpeed = new Vector3(0, 75, 0);
    public float lifeTime = 1f;
    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;
    private float timeElapsed = 0f;
    private Color startColour;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColour = textMeshPro.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;
        if (timeElapsed < lifeTime)
        {
            float fadeAlpha = startColour.a * (1 - (timeElapsed / lifeTime));
            textMeshPro.color = new Color(startColour.r, startColour.g, startColour.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
