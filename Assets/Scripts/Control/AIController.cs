using System;
using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;

        private Fighter _fighter;
        private GameObject _player;
        private Health _health;
        private Mover _mover;

        private Vector3 _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        
        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();

            _guardPosition = transform.position;
        }

        private void Update()
        {   
            // Don't do anything cause you were dead
            if (_health.GetIsDead()) return;
            
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                // Attack state
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < suspicionTime)
            {
                // suspicion state
                SuspicionBehaviour();
            }
            else
            {
                // Back to guard post
                GuardBehaviour();
            }
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            if (!_mover.IsMoving())
            {
                _timeSinceLastSawPlayer += Time.deltaTime;
            }
        }

        private void GuardBehaviour()
        {
            _mover.StartMoveAction(_guardPosition);
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < chaseDistance;
        }
    
        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
