namespace OneWeekendRayTracer {
    using System;

    public abstract class Material {
        public abstract bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered);

        private static readonly Random Rand = new Random();
        public static Vector3 RandomInUnitSphere() {
            Vector3 p;
            do {
                p = new Vector3((float)Rand.NextDouble(), (float)Rand.NextDouble(), (float)Rand.NextDouble()) * 2f - Vector3.UnitXYZ;
            } while (p.SquaredLength >= 1.0f);
            return p;
        }

        public static bool Refract(Vector3 v, Vector3 n, float niOverNt, out Vector3 refracted) {
            var uv = Vector3.Normalize(v);
            var dt = Vector3.Dot(uv, n);
            var discriminant = 1.0f - niOverNt * niOverNt * (1 - dt * dt);
            if (discriminant > 0) {
                refracted = (uv - n * dt) * niOverNt - n * (float)Math.Sqrt(discriminant);
                return true;
            } else {
                refracted = default(Vector3);
                return false;
            }
        }

        public static float Schlick(float cosine, float refractiveIndex) {
            var r0 = (1 - refractiveIndex) / (1 + refractiveIndex);
            r0 = r0 * r0;
            return r0 * (1 - r0) * (float)Math.Pow(1 - cosine, 5);
        }

        public static Vector3 Reflect(Vector3 v, Vector3 n) {
            return v - n * 2 * Vector3.Dot(v, n);
        }
    }

    public class Lambertian : Material {
        public Vector3 Albedo { get; }

        public Lambertian(Vector3 albedo) {
            Albedo = albedo;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
            var target = rec.P + rec.Normal + RandomInUnitSphere();
            scattered = new Ray(rec.P, target - rec.P);
            attenuation = Albedo;
            return true;
        }
    }

    public class Metal : Material {
        public Vector3 Albedo { get; }
        public float Fuzz { get; }

        public Metal(Vector3 albedo, float fuzz) {
            Albedo = albedo;
            Fuzz = fuzz < 1 ? fuzz : 1;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
            var reflected = Reflect(Vector3.Normalize(rIn.Direction), rec.Normal);
            scattered = new Ray(rec.P, reflected + RandomInUnitSphere() * Fuzz);
            attenuation = Albedo;
            return Vector3.Dot(scattered.Direction, rec.Normal) > 0;
        }
    }

    public class Dialectric : Material {
        public Random rand = new Random();
        public float RefractiveIndex { get; }

        public Dialectric(float refractiveIndex) {
            RefractiveIndex = refractiveIndex;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
            Vector3 outwardNormal;
            var reflected = Reflect(rIn.Direction, rec.Normal);
            float niOverNt;
            attenuation = new Vector3(1, 1, 1);
            float cosine;
            float reflectProb;


            if (Vector3.Dot(rIn.Direction, rec.Normal) > 0) {
                outwardNormal = -rec.Normal;
                niOverNt = RefractiveIndex;
                cosine = Vector3.Dot(rIn.Direction, rec.Normal) / rIn.Direction.Length;
                cosine = (float)Math.Sqrt(1.0f - RefractiveIndex * RefractiveIndex * (1 - cosine * cosine));
            } else {
                outwardNormal = rec.Normal;
                niOverNt = 1.0f / RefractiveIndex;
                cosine = -Vector3.Dot(rIn.Direction, rec.Normal) / rIn.Direction.Length;
            }
            if (Refract(rIn.Direction, outwardNormal, niOverNt, out Vector3 refracted)) {
                reflectProb = Schlick(cosine, RefractiveIndex);
            } else {
                reflectProb = 1.0f;
            }
            if (rand.NextDouble() < reflectProb) {
                scattered = new Ray(rec.P, reflected);
            } else {
                scattered = new Ray(rec.P, refracted);
            }

            return true;
        }
    }
}