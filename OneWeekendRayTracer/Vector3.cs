namespace OneWeekendRayTracer {
    using System;

    public struct Vector3 {
        private readonly float _x;
        private readonly float _y;
        private readonly float _z;

        public Vector3(float x, float y, float z) {
            _x = x;
            _y = y;
            _z = z;
        }

        public float X => _x;
        public float Y => _y;
        public float Z => _z;
        public float R => _x;
        public float G => _y;
        public float B => _z;

        public static Vector3 operator -(Vector3 v) => new Vector3(-v._x, -v._y, -v._z);


        public float this[byte i] => i == 0 ? _x : i == 1 ? _y : _z;

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z);
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z);
        public static Vector3 operator *(Vector3 v1, Vector3 v2) => new Vector3(v1._x * v2._x, v1._y * v2._y, v1._z * v2._z);
        public static Vector3 operator /(Vector3 v1, Vector3 v2) => new Vector3(v1._x / v2._x, v1._y / v2._y, v1._z / v2._z);
        public static Vector3 operator *(Vector3 v, float f) => new Vector3(v._x * f, v._y * f, v._z * f);
        public static Vector3 operator /(Vector3 v, float f) => new Vector3(v._x / f, v._y / f, v._z / f);

        public float Length => (float)Math.Sqrt(_x * _x + _y * _y + _z * _z);
        public float SquaredLength => _x * _x + _y * _y + _z * _z;

        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
        public static readonly Vector3 UnitXY = new Vector3(1, 1, 0);
        public static readonly Vector3 UnitXYZ = new Vector3(1, 1, 1);
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

        public static float Dot(Vector3 v1, Vector3 v2) => v1._x * v2._x + v1._y * v2._y + v1._z * v2._z;

        public static Vector3 Cross(Vector3 v1, Vector3 v2) =>
            new Vector3(
                        v1._y * v2._z - v1._z * v2._y,
                        -(v1._x * v2._z - v1._z * v2._x),
                        v1._x * v2._y - v1._y * v2._x
                       );

        public static Vector3 Normalize(Vector3 v) {
            return v / v.Length;
        }

        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float prc) => v1 * (1.0f - prc) + v2 * prc;
    }

    public static class Colors {
        public static readonly Vector3 White = Vector3.UnitXYZ;
    }
}