using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    public static class Collider3D
    {
        public static bool Intersection(AEntity e1, AEntity e2)
        {
            for (var i = 0; i < e1.Model.Meshes.Count; i++)
            {
                var e1BoundingSphere = e1.Model.Meshes[i].BoundingSphere;
                e1BoundingSphere.Center += e1.Position;

                for (var j = 0; j < e2.Model.Meshes.Count; j++)
                {
                    var e2BoundingSphere = e2.Model.Meshes[j].BoundingSphere;
                    e2BoundingSphere.Center += e2.Position;

                    if (e1BoundingSphere.Intersects(e2BoundingSphere))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static BoundingSphere[] UpdateBoundingSphere(AEntity e)
        {
            BoundingSphere[] spheres = new BoundingSphere[e.Model.Meshes.Count];
            for (var i = 0; i < e.Model.Meshes.Count; i++)
            {
                var eBoundingSphere = e.Model.Meshes[i].BoundingSphere;
                eBoundingSphere.Center += e.Position;
                spheres[i] = eBoundingSphere;
            }
            return spheres;
        }
        public static bool IntersectionSphere(AEntity e1, AEntity e2)
        {
            for (var i = 0; i < e1.Spheres.Length; i++)
            {
                for (var j = 0; j < e2.Spheres.Length; j++)
                {
                    if (e1.Spheres[i].Intersects(e2.Spheres[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool BoundingFrustumIntersection(AEntity e1)
        {
            BoundingFrustum boundingFrustum = new BoundingFrustum(Global.Camera.ViewMatrix * Global.Camera.ProjectionMatrix);
            for (var i = 0; i < e1.Model.Meshes.Count; i++)
            {
                var e1BoundingSphere = e1.Model.Meshes[i].BoundingSphere;
                e1BoundingSphere.Center += e1.Position;

                if (boundingFrustum.Contains(e1BoundingSphere) != ContainmentType.Disjoint)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
