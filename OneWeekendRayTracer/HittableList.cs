namespace OneWeekendRayTracer {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HittableList : Hittable {
        public IReadOnlyList<Hittable> List { get; }

        public HittableList(params Hittable[] items) {
            List = items.ToList().AsReadOnly();
        }

        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec) {
            rec = default(HitRecord);
            var hitAnything = false;
            var closestSoFar = tMax;
            foreach (var hittable in List) {
                if (!hittable.Hit(r, tMin, closestSoFar, out HitRecord tempRecord)) {
                    continue;
                }
                hitAnything = true;
                closestSoFar = tempRecord.T;
                rec = tempRecord;
            }
            return hitAnything;
        }

        public static HittableList RandomScene() {
            var rand = new Random();
            var spheres = new List<Hittable>();
            spheres.Add(new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f))));
            for (var a = -11; a < 11; a++) {
                for (var b = -11; b < 11; b++) {
                    var mat = rand.Next(100);
                    var center = new Vector3(a + 0.9f * (float)rand.NextDouble(), 0.2f, b + 0.9f * (float)rand.NextDouble());
                    if ((center - new Vector3(4, 0.2f, 0)).Length > 0.9f) {
                        if (mat < 80) {
                            spheres.Add(new Sphere(center, 0.2f, new Lambertian(new Vector3((float)rand.NextDouble() * (float)rand.NextDouble(), (float)rand.NextDouble() * (float)rand.NextDouble(), (float)rand.NextDouble() * (float)rand.NextDouble()))));
                        } else if (mat < 95) {
                            spheres.Add(new Sphere(center, 0.2f, new Metal(new Vector3(0.5f * (1 + (float)rand.NextDouble()), 0.5f * (1 + (float)rand.NextDouble()), 0.5f * (1 + (float)rand.NextDouble())), 0.5f * (float)rand.NextDouble())));
                        } else {
                            spheres.Add(new Sphere(center, 0.2f, new Dialectric(1.5f)));
                        }
                    }
                }
            }
            spheres.Add(new Sphere(new Vector3(0, 1, 0), 1.0f, new Dialectric(1.5f)));
            spheres.Add(new Sphere(new Vector3(-4, 1, 0), 1.0f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f))));
            spheres.Add(new Sphere(new Vector3(4, 1, 0), 1, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0)));

            return new HittableList(spheres.ToArray());
        }
    }
}