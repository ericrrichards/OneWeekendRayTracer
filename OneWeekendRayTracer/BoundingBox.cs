using System;

namespace OneWeekendRayTracer {
    public class BoundingBox {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public BoundingBox(Vector3 min, Vector3 max) {
            Max = max;
            Min = min;
        }

        public static BoundingBox SurroundingBox(BoundingBox a, BoundingBox b) {
            var small = new Vector3(
                Math.Min(a.Min.X, b.Min.X),
                Math.Min(a.Min.Y, b.Min.Y),
                Math.Min(a.Min.Z, b.Min.Z)
            );
            var big = new Vector3(
                Math.Max(a.Max.X, b.Max.X),
                Math.Max(a.Max.Y, b.Max.Y),
                Math.Max(a.Max.Z, b.Max.Z)
            );
            return new BoundingBox(small, big);
        }

        public bool Hit(Ray r, float tMin, float tMax) {
            for (byte a = 0; a < 3; a++) {
                var t0 = Math.Min(
                    (Min[a] - r.Origin[a]) / r.Direction[a],
                    (Max[a] - r.Origin[a]) / r.Direction[a]);
                var t1 = Math.Max(
                    (Min[a] - r.Origin[a]) / r.Direction[a],
                    (Max[a] - r.Origin[a]) / r.Direction[a]);
                tMin = Math.Max(t0, tMin);
                tMax = Math.Min(t1, tMax);
                if (tMax <= tMin) {
                    return false;
                }
            }
            return true;
        }
    }
}