// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySpace
{
    public class Entity_Monster : Entity_Base
    {
        public override void Move(float _speed) { return; }
        public override void Rotation(float _x, float _z) { return; }
        public override void Jump(float _jumpForce) { return; }
        public override void Damaged(float _damage) { return; }
        public override int Attack(float _atk) { return 0; }
    }

}