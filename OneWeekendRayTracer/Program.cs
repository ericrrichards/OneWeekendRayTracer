using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWeekendRayTracer {
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    using JetBrains.Annotations;

    class Program {
        private static readonly List<MethodInfo> Programs = typeof(Program).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name != "Main" && m.Name != "ShowMenu" && !m.Name.Contains("__"))
            .ToList();

        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (true) {
                ShowMenu();
            }
        }

        private static void ShowMenu() {
            Console.Clear();
            Console.WriteLine("Select example (Q to exit):");
            for (var i = 0; i < Programs.Count; i++) {
                var program = Programs[i];
                Console.WriteLine($"{i + 1}) {program.Name}");
            }
            prompt:
            Console.Write("> ");
            var readLine = (Console.ReadLine() ?? string.Empty).Trim();
            if (readLine == "Q" || readLine == "q") {
                Environment.Exit(0);
            } else if (string.IsNullOrWhiteSpace(readLine)) {
                goto prompt;
            }
            var choice = Convert.ToInt32(readLine);
            if (choice > 0 && choice <= Programs.Count) {
                var stopwatch = Stopwatch.StartNew();
                Programs[choice - 1].Invoke(null, null);
                stopwatch.Stop();
                Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds} ms");
                Console.ReadLine();
            } else {
                Console.WriteLine($"Invalid selection: {choice}");
                goto prompt;
            }
        }

        [UsedImplicitly]
        private static void HelloWorld() {
            var width = 200;
            var height = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var r = (float)x / width;
                    var g = (float)y / height;
                    var b = 0.2f;
                    var ir = (int)(255.99f * r);
                    var ig = (int)(255.99f * g);
                    var ib = (int)(255.99f * b);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void HelloWorldVector() {
            var width = 200;
            var height = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = new Vector3((float)x / width, (float)y / height, 0.2f);
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void Rays() {
            Vector3 Color(Ray r) {
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var lowerLeft = new Vector3(-2, -1, -1);
            var horizontal = new Vector3(4, 0, 0);
            var vertical = new Vector3(0, 2, 0);
            var origin = Vector3.Zero;

            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var u = (float)x / width;
                    var v = (float)y / height;
                    var r = new Ray(origin, lowerLeft + horizontal * u + vertical * v);
                    var col = Color(r);
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void SimpleSphere() {
            bool HitSphere(Vector3 center, float radius, Ray r) {
                var oc = r.Origin - center;
                var a = Vector3.Dot(r.Direction, r.Direction);
                var b = 2.0f * Vector3.Dot(oc, r.Direction);
                var c = Vector3.Dot(oc, oc) - radius * radius;
                var discriminant = b * b - 4 * a * c;
                return discriminant > 0;
            }
            Vector3 Color(Ray r) {
                if (HitSphere(new Vector3(0, 0, -1), 0.5f, r)) {
                    return new Vector3(1, 0, 0);
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var lowerLeft = new Vector3(-2, -1, -1);
            var horizontal = new Vector3(4, 0, 0);
            var vertical = new Vector3(0, 2, 0);
            var origin = Vector3.Zero;

            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var u = (float)x / width;
                    var v = (float)y / height;
                    var r = new Ray(origin, lowerLeft + horizontal * u + vertical * v);
                    var col = Color(r);
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void ShadedSphere() {
            float HitSphere(Vector3 center, float radius, Ray r) {
                var oc = r.Origin - center;
                var a = Vector3.Dot(r.Direction, r.Direction);
                var b = 2.0f * Vector3.Dot(oc, r.Direction);
                var c = Vector3.Dot(oc, oc) - radius * radius;
                var discriminant = b * b - 4 * a * c;
                if (discriminant < 0) {
                    return -1;
                }
                return (-b - (float)Math.Sqrt(discriminant)) / (2f * a);
            }
            Vector3 Color(Ray r) {
                var t = HitSphere(new Vector3(0, 0, -1), 0.5f, r);

                if (t > 0.0) {
                    var n = Vector3.Normalize(r.At(t) - new Vector3(0, 0, -1));
                    return new Vector3(n.X + 1, n.Y + 1, n.Z + 1) * 0.5f;
                }

                var unitDirection = Vector3.Normalize(r.Direction);
                t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var lowerLeft = new Vector3(-2, -1, -1);
            var horizontal = new Vector3(4, 0, 0);
            var vertical = new Vector3(0, 2, 0);
            var origin = Vector3.Zero;

            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var u = (float)x / width;
                    var v = (float)y / height;
                    var r = new Ray(origin, lowerLeft + horizontal * u + vertical * v);
                    var col = Color(r);
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void TwoSpheres() {
            Vector3 Color(Ray r, Hittable hittable) {
                if (hittable.Hit(r, 0.0f, float.MaxValue, out HitRecord rec)) {
                    return new Vector3(rec.Normal.X + 1, rec.Normal.Y + 1, rec.Normal.Z + 1) * 0.5f;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var lowerLeft = new Vector3(-2, -1, -1);
            var horizontal = new Vector3(4, 0, 0);
            var vertical = new Vector3(0, 2, 0);
            var origin = Vector3.Zero;

            var world = new HittableList(new Sphere(new Vector3(0, 0, -1), 0.5f), new Sphere(new Vector3(0, -100.5f, -1), 100));

            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var u = (float)x / width;
                    var v = (float)y / height;
                    var r = new Ray(origin, lowerLeft + horizontal * u + vertical * v);
                    var col = Color(r, world);
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void AntiAliased() {
            Vector3 Color(Ray r, Hittable hittable) {
                if (hittable.Hit(r, 0.0f, float.MaxValue, out HitRecord rec)) {
                    return new Vector3(rec.Normal.X + 1, rec.Normal.Y + 1, rec.Normal.Z + 1) * 0.5f;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new HittableList(new Sphere(new Vector3(0, 0, -1), 0.5f), new Sphere(new Vector3(0, -100.5f, -1), 100));
            var rand = new Random();
            var cam = new Camera();
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world);
                    }
                    col /= samples;
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void Diffuse() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable) {
                if (hittable.Hit(r, 0.0f, float.MaxValue, out HitRecord rec)) {
                    var target = rec.P + rec.Normal + Material.RandomInUnitSphere();
                    return Color(new Ray(rec.P, target - rec.P), hittable) * 0.5f;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new HittableList(new Sphere(new Vector3(0, 0, -1), 0.5f), new Sphere(new Vector3(0, -100.5f, -1), 100));

            var cam = new Camera();
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void Metal() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new HittableList(
                new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new ConstantTexture(new Vector3(0.8f, 0.3f, 0.3f)))),
                new Sphere(new Vector3(0, -100.5f, -1), 100, new Lambertian(new ConstantTexture(new Vector3(0.8f, 0.8f, 0.0f)))),
                new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new ConstantTexture( new Vector3(0.8f, 0.6f, 0.2f)), 1.0f)),
                new Sphere(new Vector3(-1, 0, -1), 0.5f, new Metal(new ConstantTexture( new Vector3(0.8f, 0.8f, 0.8f)), 0.3f))
            );

            var cam = new Camera();
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void Dialectric() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new HittableList(
                                         new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new ConstantTexture(new Vector3(0.1f, 0.2f, 0.5f)))),
                                         new Sphere(new Vector3(0, -100.5f, -1), 100, new Lambertian(new ConstantTexture(new Vector3(0.8f, 0.8f, 0.0f)))),
                                         new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new ConstantTexture( new Vector3(0.8f, 0.6f, 0.2f)), 0)),
                                         new Sphere(new Vector3(-1, 0, -1), 0.5f, new Dialectric(1.5f)),
                                         new Sphere(new Vector3(-1, 0, -1), -0.45f, new Dialectric(1.5f))
                                        );

            var cam = new Camera();
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        
        [UsedImplicitly]
        private static void Camera() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");
            var R = (float)Math.Cos(Math.PI / 4);
            var world = new HittableList(
                                         new Sphere(new Vector3(-R, 0, -1), R, new Lambertian(new ConstantTexture(new Vector3(0, 0, 1)))),
                                         new Sphere(new Vector3(R, 0, -1), R, new Lambertian(new ConstantTexture(new Vector3(1, 0, 0))))
                                        );

            var cam = new Camera(90, (float)width / height);
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void Camera2() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new HittableList(
                                         new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new ConstantTexture(new Vector3(0.1f, 0.2f, 0.5f)))),
                                         new Sphere(new Vector3(0, -100.5f, -1), 100, new Lambertian(new ConstantTexture(new Vector3(0.8f, 0.8f, 0.0f)))),
                                         new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new ConstantTexture(new Vector3(0.8f, 0.6f, 0.2f)), 0)),
                                         new Sphere(new Vector3(-1, 0, -1), 0.5f, new Dialectric(1.5f)),
                                         new Sphere(new Vector3(-1, 0, -1), -0.45f, new Dialectric(1.5f))
                                        );

            var cam = new Camera(new Vector3(-2, 2, 1), new Vector3(0, 0, -1), Vector3.UnitY, 30, (float)width / height);
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void DefocusBlur() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 200;
            var height = 100;
            var samples = 100;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new HittableList(
                                         new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new ConstantTexture(new Vector3(0.1f, 0.2f, 0.5f)))),
                                         new Sphere(new Vector3(0, -100.5f, -1), 100, new Lambertian(new ConstantTexture(new Vector3(0.8f, 0.8f, 0.0f)))),
                                         new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new ConstantTexture( new Vector3(0.8f, 0.6f, 0.2f)), 0)),
                                         new Sphere(new Vector3(-1, 0, -1), 0.5f, new Dialectric(1.5f)),
                                         new Sphere(new Vector3(-1, 0, -1), -0.45f, new Dialectric(1.5f))
                                        );

            var lookFrom = new Vector3(3, 3, 2);
            var lookAt = new Vector3(0, 0, -1);
            var distToFocus = (lookFrom - lookAt).Length;
            var aperature = 2.0f;
            var cam = new Camera(lookFrom, lookAt, Vector3.UnitY, 20, (float)width / height, aperature, distToFocus);
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void RandomScene() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 1200;
            var height = 800;
            var samples = 10;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = HittableList.RandomScene();

            var lookFrom = new Vector3(13, 2, 3);
            var lookAt = Vector3.Zero;
            var distToFocus = 10.0f;
            var aperature = 0.1f;
            var cam = new Camera(lookFrom, lookAt, Vector3.UnitY, 20, (float)width / height, aperature, distToFocus);
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
        [UsedImplicitly]
        private static void RandomSceneBVH() {
            var rand = new Random();


            Vector3 Color(Ray r, Hittable hittable, int depth) {
                if (hittable.Hit(r, 0.001f, float.MaxValue, out HitRecord rec)) {
                    if (depth < 50 && rec.Material.Scatter(r, rec, out Vector3 attenuation, out Ray scattered)) {
                        return attenuation * Color(scattered, hittable, depth + 1);
                    }
                    return Vector3.Zero;
                }
                var unitDirection = Vector3.Normalize(r.Direction);
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return Vector3.Lerp(Colors.White, new Vector3(0.5f, 0.7f, 1.0f), t);
            }

            var width = 1200;
            var height = 800;
            var samples = 10;

            var sb = new StringBuilder();
            sb.Append($"P3\r\n{width} {height}\r\n255\r\n");

            var world = new BVHNode(HittableList.RandomScene().List, 0, 0);

            var lookFrom = new Vector3(13, 2, 3);
            var lookAt = Vector3.Zero;
            var distToFocus = 10.0f;
            var aperature = 0.1f;
            var cam = new Camera(lookFrom, lookAt, Vector3.UnitY, 20, (float)width / height, aperature, distToFocus);
            for (var y = height - 1; y >= 0; y--) {
                for (var x = 0; x < width; x++) {
                    var col = Vector3.Zero;
                    for (var s = 0; s < samples; s++) {
                        var u = (float)(x + rand.NextDouble()) / width;
                        var v = (float)(y + rand.NextDouble()) / height;
                        var r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }
                    col /= samples;
                    col = new Vector3((float)Math.Sqrt(col.R), (float)Math.Sqrt(col.G), (float)Math.Sqrt(col.B));
                    var ir = (int)(255.99f * col.R);
                    var ig = (int)(255.99f * col.G);
                    var ib = (int)(255.99f * col.B);

                    sb.AppendLine($"{ir} {ig} {ib}");
                }
            }
            var fileName = "out.ppm";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
    }


}
