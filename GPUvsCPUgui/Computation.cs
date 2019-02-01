using System;
using System.Diagnostics;
using Alea;
using Alea.Parallel;


namespace GPUvsCPUgui
{
    public struct Vector
    {
        public byte x, y;

        public Vector(byte x, byte y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Points
    {
        public Vector start, end;

        public Points(Vector start, Vector end)
        {
            this.start = start;
            this.end = end;
        }
    }

    struct Result
    {
        public double angle, distance;

        public Result(double angle, double distance)
        {
            this.angle = angle;
            this.distance = distance;
        }                        
    }

    public class Computation
    {

        private int length;
        private Random random;
        private Stopwatch watch;
        private static Gpu gpu = Gpu.Default;

        public Computation(int numberOfIterations)
        {
            length = numberOfIterations;
            random = new Random();
            watch = new Stopwatch();
        }

        private void RandomizePoints(ref Points[] pointsArray)
        {
            for (int i = 0; i < length; i++)
            {
                Vector startPoint = new Vector((byte)random.Next(0, 255), (byte)random.Next(0, 255));
                Vector endPoint = new Vector((byte)random.Next(0, 255), (byte)random.Next(0, 255));

                pointsArray[i] = new Points(startPoint, endPoint);
            }
        }

        private static void Compute(Points points)
        {
            double angle = Formula.Angle(points);
            double distance = Formula.Distance(points);
        }

        public long MeasureCPUComputationTime()
        {
            Points[] pointsArray = new Points[length];
            RandomizePoints(ref pointsArray);

            watch = Stopwatch.StartNew();

            for (int i = 0; i < length; i++)
            {
                Compute(pointsArray[i]);
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long MeasureGPUComputationTime()
        {
            Points[] pointsArray = new Points[length];
            RandomizePoints(ref pointsArray);

            watch = Stopwatch.StartNew();

            gpu.For(0, length, i => 
            {
                Compute(pointsArray[i]);
            });

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }



        

    }
}
