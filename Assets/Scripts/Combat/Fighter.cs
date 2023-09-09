using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Core;
using UnityEngine;
using Movement;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttack = 1f;
        [SerializeField] private float weaponDamage = 5f;
        
        private Health _target;
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private float _timeSinceLastAttack = 0;
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;

            if (_target.GetIsDead()) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {   
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > timeBetweenAttack)
            {
                // This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger(Attack1);
                _timeSinceLastAttack = 0;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {   
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
            print("Come get some");
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.GetIsDead();
        }

        public void Cancel()
        {   
            GetComponent<Animator>().SetTrigger(StopAttack);
            _target = null;
        }
        
        // Animation Event
        void Hit()
        {
            if (_target == null)
            {
                return;
            }
            
            _target.TakeDamage(weaponDamage);
        }
    }
}

