namespace OneWeekendRayTracer {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BVHNode : Hittable {
        public Hittable Left { get; }
        public Hittable Right { get; }
        private readonly BoundingBox _box;
        private static Random random;
        

        public BVHNode(IEnumerable<Hittable> l, float t0, float t1) {
            random = new Random();
            var sorted = SortHittablesByRandomAxis(l);
            if (sorted.Count == 1) {
                Left = Right = sorted[0];
            } else if (sorted.Count == 2) {
                Left = sorted[0];
                Right = sorted[1];
            } else {
                Left = new BVHNode(sorted.Take(sorted.Count/2).ToList(), t0, t1);
                Right = new BVHNode(sorted.Skip(sorted.Count/2).ToList(), t0, t1);
            }
            if (!Left.BoundingBox(t0, t1, out BoundingBox boxLeft) || !Right.BoundingBox(t0, t1, out BoundingBox boxRight)) {
                throw new InvalidOperationException("Missing bounding box!");
            }
            _box = OneWeekendRayTracer.BoundingBox.SurroundingBox(boxLeft, boxRight);
        }

        private static List<Hittable> SortHittablesByRandomAxis(IEnumerable<Hittable> l) {
            var axis = random.Next(3);
            switch (axis) {
                case 0:
                    return l.OrderBy(
                                     h => {
                                         if (!h.BoundingBox(0, 0, out BoundingBox box)) {
                                             throw new InvalidOperationException("Hittable does not have a bounding box!");
                                         }
                                         return box.Min.X;
                                     })
                            .ToList();
                    
                case 1:
                    return l.OrderBy(
                                     h => {
                                         if (!h.BoundingBox(0, 0, out BoundingBox box)) {
                                             throw new InvalidOperationException("Hittable does not have a bounding box!");
                                         }
                                         return box.Min.Y;
                                     })
                            .ToList();
                    
                case 2:
                    return l.OrderBy(
                                     h => {
                                         if (!h.BoundingBox(0, 0, out BoundingBox box)) {
                                             throw new InvalidOperationException("Hittable does not have a bounding box!");
                                         }
                                         return box.Min.Z;
                                     })
                            .ToList();
                    ;
                default:
                    throw new InvalidOperationException($"Invalid axis index: {axis}");
            }
        }

        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec) {
            rec = default(HitRecord);
            if (!_box.Hit(r, tMin, tMax))
                return false;
            var hitLeft = Left.Hit(r, tMin, tMax, out HitRecord leftRec);
            var hitRight = Right.Hit(r, tMin, tMax, out HitRecord rightRec);
            if (hitLeft && hitRight) {
                rec = leftRec.T < rightRec.T ? leftRec : rightRec;
                return true;
            }
            if (hitLeft) {
                rec = leftRec;
                return true;
            }
            if (hitRight) {
                rec = rightRec;
                return true;
            }
            return false;
        }

        public override bool BoundingBox(float t0, float t1, out BoundingBox box) {
            box = _box;
            return true;
        }
    }
}