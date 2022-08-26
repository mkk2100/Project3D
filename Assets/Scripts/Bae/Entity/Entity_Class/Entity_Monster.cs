// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySpace
{
    public class Entity_Monster : Entity_Base
    {
        public override bool Move(float _speed) { return false; }
        public override bool Rotation(float _x, float _z) { return false; }
        public override bool Jump(float _jumpForce) { return false; }
        public override float Damaged(float _damage) { return 0; }
        public override bool Attack(float _atk) { return false; }
    }

}