namespace OneWeekendRayTracer {
    using System;

    public class Sphere : Hittable {
        public Vector3 Center { get; }
        public float Radius { get; }
        public Material Material { get; }

        public Sphere(Vector3 center, float radius, Material material=null) {
            Center = center;
            Radius = radius;
            Material = material;
        }

        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec) {
            var oc = r.Origin - Center;
            var a = Vector3.Dot(r.Direction, r.Direction);
            var b = Vector3.Dot(oc, r.Direction);
            var c = Vector3.Dot(oc, oc) - Radius * Radius;
            var discriminant = b * b - a * c;
            if (discriminant > 0) {
                var temp = (-b - (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin) {
                    var p = r.At(temp);
                    rec = new HitRecord { T = temp, P = p, Normal = (p - Center) / Radius, Material = Material};
                    return true;
                }
                temp = (-b + (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin) {
                    var p = r.At(temp);
                    rec = new HitRecord { T = temp, P = p, Normal = (p - Center) / Radius, Material = Material};
                    return true;
                }
            }
            rec = default(HitRecord);
            return false;
        }
    }
}