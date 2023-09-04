using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Movement;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        private Transform _target;
        
        private void Update()
        {
            if (_target == null) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {   
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.transform;
            print("Come get some");
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}

