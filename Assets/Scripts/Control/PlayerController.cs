using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {   
    
        private Camera _camera;
        private Mover _mover;

        // Start is called before the first frame update
        void Start()
        {
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }
    
        private void MoveToCursor()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {   
                _mover.MoveTo(hit.point);
            }
        }
    }
}

