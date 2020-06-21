using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Deathfrog()
    {
        anim.SetTrigger("Death");
    }
    private void death()
    {
        Destroy(this.gameObject);
    }
}
