using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Crator : MonoBehaviour
{
    public float timeToDisapear;
    bool startFade;
    public SpriteRenderer sprite;
    public Color alpha;
    public Color alpha2;
    public float fadeSpeed;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

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
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.9f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.8f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.7f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.1f);
        yield return new WaitForSeconds(fadeSpeed);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        PV.RPC("RPC_DestroyMe", RpcTarget.AllBuffered);
        StopCoroutine(Faded());
    }


    [PunRPC]
    void RPC_DestroyMe()
    {
        Destroy(this.gameObject);
    }


}
