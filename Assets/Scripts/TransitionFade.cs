using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFade : MonoBehaviour
{
    private Color alphaColor;
    private float timeToFade = 1.2f;
    public bool canFade;
    // Start is called before the first frame update
    void Start()
    {
        canFade = false;
        alphaColor = gameObject.GetComponent<Renderer>().material.color;
        alphaColor.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canFade)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(gameObject.GetComponent<Renderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
        }
    }
}
