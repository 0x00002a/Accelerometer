using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VRageMath;

namespace Natomic.Accelerometer
{
    public enum AccelUnit
    {
        Metric,
        GForce,
    }
    public class Config
    {
        private static Vector2D DEFAULT_POS = new Vector2D(-0.64, -0.64);


        [DefaultValue(AccelUnit.Metric)]
        public AccelUnit Unit;

        // C# is very limited when it comes to user-defined types apparently 
        internal double position_x_ = -0.64;

        internal double position_y_ = -0.64;

        public Vector2D Position
        {
            set { position_x_ = value.X; position_y_ = value.Y; }
            get { return new Vector2D(position_x_, position_y_); }
        }


    }
}
