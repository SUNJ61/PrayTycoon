using System;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        private JoyStick Joystick;

        private Action MoveLogic;

        private void Start()
        {
            animator = GetComponent<Animator>();

            #if UNITY_EDITOR || UNITY_ANDROID
                if (UIManager.Instance != null && UIManager.Instance.JoyStickUI != null)
                {
                    UIManager.Instance.JoyStickUI.gameObject.SetActive(true);
                    Joystick = UIManager.Instance.JoyStickUI;
                    MoveLogic = MobileMove;
                }
            #else
                UIManager.Instance.JoyStickUI.gameObject.SetActive(false);
                MoveLogic = PCMove;
            #endif
        }


        private void Update()
        {
            MoveLogic?.Invoke(); // Action이 null이 아니면 실행.
        }

        private void PlayerMove()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }

        private void PlayerUI()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UIManager.Instance.InventoryUIControl();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(!UIManager.Instance.MenuUI.activeSelf)
                    UIManager.Instance.UIOff();

                    UIManager.Instance.MenuUIControl();
            }
        }

        private void PCMove()
        {
            PlayerMove();
            PlayerUI();
        }

        private void MobileMove()
        {
            Vector2 dir = Vector2.zero;
            if (Joystick.Horizontal == -1)
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
            }
            else if (Joystick.Horizontal == 1)
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
            }

            if (Joystick.Vertical == 1)
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
            }
            else if (Joystick.Vertical == -1)
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }
    }
}
