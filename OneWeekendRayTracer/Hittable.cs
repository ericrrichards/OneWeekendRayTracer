namespace OneWeekendRayTracer {
    public class HitRecord {
        public readonly float T;
        public readonly Vector3 P;
        public readonly Vector3 Normal;
        public readonly Material Material;

        public HitRecord(float t, Vector3 p, Vector3 n, Material m) {
            T = t;
            P = p;
            Normal = n;
            Material = m;
        }
    }

    public abstract class Hittable {
        public abstract bool Hit(Ray r, float tMin, float tMax, out HitRecord rec);
    }
}