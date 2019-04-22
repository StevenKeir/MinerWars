using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crator : MonoBehaviour
{
    public float timeToDisapear;
    bool startFade;
    public SpriteRenderer sprite;
    public Color alpha;
    public Color alpha2;


    void Update()
    {
        WhenToFade();
    }

    void WhenToFade()
    {
        timeToDisapear -= Time.deltaTime;
        if(timeToDisapear <= 0.00f)
        {
            startFade = true;
            if (startFade)
            {
                StartCoroutine(Faded());
                startFade = false;
                timeToDisapear = 10f;
            }
        }
    }

    IEnumerator Faded()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.9f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.8f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.7f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.1f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        StopCoroutine(Faded());
    }

}
