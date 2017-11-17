namespace OneWeekendRayTracer {
    using System;

    public abstract class Texture {
        public abstract Vector3 Value(float u, float v, Vector3 p);
    }

    public class ConstantTexture : Texture {
        private readonly Vector3 _color;

        public ConstantTexture(Vector3 color) {
            _color = color;
        }

        public override Vector3 Value(float u, float v, Vector3 p) {
            return _color;
        }
    }

    public class CheckerTexture : Texture {
        private readonly Texture _even;
        private readonly Texture _odd;

        public CheckerTexture(Texture t0, Texture t1) {
            _even = t0;
            _odd = t1;
        }

        public override Vector3 Value(float u, float v, Vector3 p) {
            var sines = Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z);
            return sines < 0 ? _odd.Value(u, v, p) : _even.Value(u, v, p);
        }
    }
}