namespace OneWeekendRayTracer {
    using System;

    public struct Vector3 {
        public float[] v;

        public Vector3(float x, float y, float z) {
            v = new[] { x, y, z };
        }

        public float X => v[0];
        public float Y => v[1];
        public float Z => v[2];
        public float R => v[0];
        public float G => v[1];
        public float B => v[2];

        public static Vector3 operator -(Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);


        public float this[int i] => v[i];

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector3 operator *(Vector3 v1, Vector3 v2) => new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        public static Vector3 operator /(Vector3 v1, Vector3 v2) => new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        public static Vector3 operator*(Vector3 v, float f)=>new Vector3(v.X*f, v.Y*f, v.Z*f);
        public static Vector3 operator/(Vector3 v, float f)=> new Vector3(v.X/f, v.Y/f, v.Z/f);

        public float Length => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        public float SquaredLength => X * X + Y * Y + Z * Z;

        public static Vector3 UnitX => new Vector3(1, 0, 0);
        public static Vector3 UnitY => new Vector3(0,1,0);
        public static Vector3 UnitZ => new Vector3(0,0,1);

        public static float Dot(Vector3 v1, Vector3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => 
            new Vector3(
                        v1.Y*v2.Z - v1.Z*v2.Y,
                        -(v1.X*v2.Z - v1.Z*v2.X),
                        v1.X * v2.Y - v1.Y*v2.X
                       );

        public static Vector3 Normalize(Vector3 v) {
            return v / v.Length;
        }

        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float prc) => v1 * (1.0f - prc) + v2 * prc;
    }
}