using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [SerializeField] private StackController stack;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>(); 
    }

    public void Footstep()    // Функция ищется .fbx AnimationEvent'ами по имени во всех скриптах на объекте, на котором висит Animator
    {
        var stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        stack.FootstepAnimation(stateInfo.speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Reap")) return;
        if (Barn.Instance.CheckStackNotFull())
        {
            other.GetComponent<FlyParticleEffect>().Init(other.transform, stack.transform);    // Пнуть сноп в стак
            Barn.Instance.UpdateStack(true);
        }
    }
}
