using System;
using System.Collections.Generic;
using System.Text;

namespace FakeUnity
{
    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
