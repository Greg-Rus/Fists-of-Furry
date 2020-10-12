using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Transform _ragdolRoot;
    [SerializeField] private bool _ragdolEnabled = false;
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;
    
     void OnEnable()
     {
         _rigidbodies = _ragdolRoot.GetComponentsInChildren<Rigidbody>();
         _colliders = _ragdolRoot.GetComponentsInChildren<Collider>();
         RagdollActive(false);
     }

     public void RagdollActive(bool isEnabled)
     {
         foreach (var collider in _colliders)
         {
             collider.enabled = isEnabled;
         }

         foreach (var rigidbody in _rigidbodies)
         {
             rigidbody.isKinematic = !isEnabled;
         }
     }

     public void ApplyExplosionForce(float explosionForce, Vector3 position, float upwardsModifier)
     {
         foreach (var rigidbody in _rigidbodies)
         {
             rigidbody.AddExplosionForce(explosionForce, position, 10, upwardsModifier);
         }
     }
}
