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
        public override void Jump(float _jumpForce) { }

        public override bool Attack(float _atk) { return false; }
    }

}