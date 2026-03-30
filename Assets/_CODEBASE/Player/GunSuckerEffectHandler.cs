using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSuckerEffectHandler : MonoBehaviour
{
    [SerializeField] MeshRenderer rend;
    [SerializeField] float fadeSpeed = 1f;

    private void Awake()
    {
        GunHand.activateSunSuck += RequestEffect;
    }

    private void RequestEffect(bool status)
    {
        if (status)
        {
            ActivateEffect();
        }
        else
        {
            DeactivateEffect();
        }
    }

    private void ActivateEffect()
    {
        StartCoroutine(EffectFadeIn());
    }

    private void DeactivateEffect()
    {
        StartCoroutine(EffectFadeOut());
    }

    IEnumerator EffectFadeIn()
    {
        for (float i = 0; i < 1; i+= (fadeSpeed + 0.05f))
        {
            rend.material.SetFloat("_Fade", i);
            yield return new WaitForSeconds(fadeSpeed);
        }
        rend.material.SetFloat("_Fade", 1);
    }

    IEnumerator EffectFadeOut()
    {
        for (float i = 1; i > 0; i -= (fadeSpeed + 0.05f))
        {
            rend.material.SetFloat("_Fade", i);
            yield return new WaitForSeconds(fadeSpeed);
        }
        rend.material.SetFloat("_Fade", 0);
    }
}
