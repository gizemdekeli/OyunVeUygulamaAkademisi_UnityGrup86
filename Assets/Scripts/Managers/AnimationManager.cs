using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] bool tween;
    [SerializeField] Vector3 tweenTargetPosition;
    [SerializeField] Transform tweenObject;
    [SerializeField] float tweenDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!tween)
            {
                _animator.SetTrigger("canPlay");
            }
            else
            {
                tweenObject.transform.DOLocalMove(tweenTargetPosition, tweenDuration);
            }
        }
    }
}
