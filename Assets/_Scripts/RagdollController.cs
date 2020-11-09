using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Transform _ragdolRoot;
    [SerializeField] private GameObject _baseRigidbodyAndCollider;
#pragma warning restore 649
    private Rigidbody[] _ragdollRigidbodies;
    private Collider[] _ragdollColliders;
    private Animator _animator;

    void OnEnable()
     {
         _ragdollRigidbodies = _ragdolRoot.GetComponentsInChildren<Rigidbody>();
         _ragdollColliders = _ragdolRoot.GetComponentsInChildren<Collider>();
         _animator = GetComponent<Animator>();
         RagdollActive(false);
     }

     public void RagdollActive(bool isEnabled)
     {
         foreach (var collider in _ragdollColliders)
         {
             collider.enabled = isEnabled;
         }

         foreach (var rigidbody in _ragdollRigidbodies)
         {
             rigidbody.isKinematic = !isEnabled;
         }

         _baseRigidbodyAndCollider.SetActive(!isEnabled);
         _animator.enabled = !isEnabled;
     }

     public void ApplyExplosionForce(float explosionForce, Vector3 position, float upwardsModifier)
     {
         foreach (var rigidbody in _ragdollRigidbodies)
         {
             rigidbody.AddExplosionForce(explosionForce, position, 10, upwardsModifier);
         }
     }
}
