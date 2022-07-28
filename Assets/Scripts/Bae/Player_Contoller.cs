using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Player_Contoller : MonoBehaviour
    {
        [SerializeField]
        private Entity_Status entityStatus;
        private void Start()
        {
            entityStatus = new Entity_Status(3,1,3);
        }
        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-entityStatus.Speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(entityStatus.Speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, -entityStatus.Speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, entityStatus.Speed * Time.deltaTime);
            }
        }
    }

}