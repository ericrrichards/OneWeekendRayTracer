namespace OneWeekendRayTracer {
    public struct HitRecord {
        public float T { get; set; }
        public Vector3 P { get; set; }
        public Vector3 Normal { get; set; }
        public Material Material { get; set; }
    }

    public abstract class Hittable {
        public abstract bool Hit(Ray r, float tMin, float tMax, out HitRecord rec);
    }
}