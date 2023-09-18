using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent _navMeshAgent;
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        private Animator _animator;
        private Health _health;

        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        // Update is called once per frame
        private void Update()
        {
            _navMeshAgent.enabled = !_health.GetIsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {   
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
        
        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat(ForwardSpeed, speed);
        }

        public bool IsMoving()
        {
            return _navMeshAgent.velocity.magnitude > 0.1f;
        }
    }
}

