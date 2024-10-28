using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAnimationPlay : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void PropPlay()
    {
        animator.SetTrigger("Play");
    }


}
