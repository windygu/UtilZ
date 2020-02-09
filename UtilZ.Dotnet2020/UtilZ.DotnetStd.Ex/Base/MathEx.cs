using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base
{
    public class MathEx
    {
        public const double ANGLE_90 = 90d;
        public const double ANGLE_180 = 180d;
        public const double ANGLE_270 = 270d;
        public const double ANGLE_360 = 360d;

        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static double AngleToRadians(double angle)
        {
            return angle * Math.PI / ANGLE_180;
        }

        /// <summary>
        /// 弧度转换为角度
        /// </summary>
        /// <param name="radians">弧度</param>
        /// <returns></returns>
        public static double RadiansToAngle(double radians)
        {
            return radians * ANGLE_180 / Math.PI;
        }
    }
}
