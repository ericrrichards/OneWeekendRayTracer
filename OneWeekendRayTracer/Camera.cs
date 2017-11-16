namespace OneWeekendRayTracer {
    using System;

    public class Camera {
        private static readonly Random Rand = new Random();
        private const float Pi = (float)Math.PI;
        public readonly Vector3 Origin;
        public readonly Vector3 LowerLeft;
        public readonly Vector3 Horizontal;
        public readonly Vector3 Vertical;
        public readonly float LensRadius;
        public readonly Vector3 Look;
        public readonly Vector3 Right;
        public readonly Vector3 Up;

        public Camera() : this(90, 2) {
        }

        public Camera(float vFov, float aspect) : this(Vector3.Zero, new Vector3(0, 0, -1), Vector3.UnitY, vFov, aspect) {

        }

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 up, float vFov, float aspect, float aperture = 0, float focusDistance = 1) {
            LensRadius = aperture / 2;
            var theta = vFov * Pi / 180;
            var halfHeight = (float)Math.Tan(theta / 2);
            var halfWidth = aspect * halfHeight;
            Origin = lookFrom;
            Look = Vector3.Normalize(lookFrom - lookAt);
            Right = Vector3.Normalize(Vector3.Cross(up, Look));
            Up = Vector3.Cross(Look, Right);
            LowerLeft = Origin - Right * halfWidth * focusDistance - Up * halfHeight * focusDistance - Look * focusDistance;
            Horizontal = Right * halfWidth * 2 * focusDistance;
            Vertical = Up * halfHeight * 2 * focusDistance;
        }

        public Ray GetRay(float u, float v) {
            var rd = SampleDisk() * LensRadius;
            var offset = Right * rd.X + Up * rd.Y;
            return new Ray(Origin + offset, LowerLeft + Horizontal * u + Vertical * v - Origin - offset);
        }


        public static Vector3 SampleDisk() {
            Vector3 p;
            do {
                p = new Vector3((float)Rand.NextDouble(), (float)Rand.NextDouble(), 0) * 2 - Vector3.UnitXY;
            } while (Vector3.Dot(p, p) >= 1.0f);
            return p;
        }
    }
}