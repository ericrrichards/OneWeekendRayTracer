namespace OneWeekendRayTracer {
    public class Ray {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction) {
            Origin = origin;
            Direction = direction;
        }

        public Vector3 At(float t) => Origin + Direction * t;
    }
}