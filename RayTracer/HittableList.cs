using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    internal class HittableList : Hittable
    {

        private List<Hittable> objects;
        public HittableList()
        {
            objects = new List<Hittable>();
        }
        public HittableList(List<Hittable> objects)
        {
            this.objects = objects;
        }
        public void Clear()
        {
            objects.Clear();
        }
        public void Add(Hittable objectToAdd)
        {
            objects.Add(objectToAdd);
        }

        public bool Hit(Ray r, Interval rayT, ref HitRecord rec)
        {
            HitRecord tempRec = new HitRecord();

            bool hitAnything = false;
            double closestSoFar = rayT.Max;

            foreach (Hittable objectToCheck in objects)
            {
                if (objectToCheck.Hit(r, new Interval(rayT.Min, closestSoFar), ref tempRec))
                {
                    hitAnything = true;
                    closestSoFar = tempRec.T;
                    rec = tempRec;
                }
            }
            return hitAnything;
        }
    }
}
