using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Eject : MonoBehaviour
    {
        public float springForce = 10.0f;
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Movement playerMovement = collision.GetComponent<Movement>();
                playerMovement.Jump(springForce);
                _anim.SetTrigger("extend");
            }
        }
    }
}
