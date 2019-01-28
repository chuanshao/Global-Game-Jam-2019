﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Fade in", true);
        }    
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Fade in", false);
        }
    }

    public void AnimFF()
    {
        StartCoroutine(Play());
    }

    IEnumerator  Play()
    {
        anim.SetBool("Fade in", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("Fade in", false);
    }

}
