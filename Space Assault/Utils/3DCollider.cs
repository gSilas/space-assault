using Space_Assault.Entities;

namespace Space_Assault.Utils
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
        }
}
