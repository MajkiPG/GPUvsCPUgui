using System;
using Alea;

namespace GPUvsCPUgui
{
    public static class Formula
    {
        public static double Distance(Points points)
        {
            double pow1 = DeviceFunction.Pow(points.end.x - points.start.x, 2);
            double pow2 = DeviceFunction.Pow(points.end.y - points.start.y, 2);

            return Math.Sqrt(pow1 + pow2);
        }

        public static double Angle(Points points)
        {
            if (points.start.x < points.end.x)
            {
                if (points.start.y > points.end.y)
                {
                    return Math.Atan((points.end.x - points.start.x) / (points.start.y - points.end.y)) * (180 / Math.PI);         //I QUARTER
                }
                else if (points.start.y < points.end.y)
                {
                    return Math.Atan((points.end.y - points.start.y) / (points.end.x - points.start.x)) * (180 / Math.PI) + 89;         //IV QUARTER
                }
                else
                {
                    return 90;
                }
            }
            else if (points.start.x > points.end.x)
            {
                if (points.start.y < points.end.y)
                {
                    return Math.Atan((points.start.x - points.end.x) / (points.end.y - points.start.y)) * (180 / Math.PI) + 179;         //III QUARTER
                }
                else if (points.start.y > points.end.y)
                {
                    return Math.Atan((points.start.y - points.end.y) / (points.start.x - points.end.x)) * (180 / Math.PI) + 269;         //II QUARTER
                }
                else
                {
                    return 270;
                }
            }
            else
            {
                if (points.start.y > points.end.y)
                {
                    return 0;
                }
                else
                {
                    return 180;
                }
            }
        }
    }
}
