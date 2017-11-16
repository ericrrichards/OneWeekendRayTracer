namespace OneWeekendRayTracer {
    using System;

    public class Sphere : Hittable {
        public readonly Vector3 Center;
        public readonly float Radius;
        public readonly float RadiusSquared;
        public readonly Material Material;

        public Sphere(Vector3 center, float radius, Material material = null) {
            Center = center;
            Radius = radius;
            RadiusSquared = radius * radius;
            Material = material;
        }

        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec) {
            var oc = r.Origin - Center;
            var a = Vector3.Dot(r.Direction, r.Direction);
            var b = Vector3.Dot(oc, r.Direction);
            var c = Vector3.Dot(oc, oc) - RadiusSquared;
            var discriminant = b * b - a * c;
            if (discriminant > 0) {
                var sqrt = (float)Math.Sqrt(discriminant);
                var temp = (-b - sqrt) / a;
                if (temp < tMax && temp > tMin) {
                    var p = r.At(temp);
                    rec = new HitRecord(temp, p, (p - Center) / Radius, Material);
                    return true;
                }
                temp = (-b + sqrt) / a;
                if (temp < tMax && temp > tMin) {
                    var p = r.At(temp);
                    rec = new HitRecord(temp, p, (p - Center) / Radius, Material);
                    return true;
                }
            }
            rec = default(HitRecord);
            return false;
        }
    }
}