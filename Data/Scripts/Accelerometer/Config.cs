using Sandbox.ModAPI;
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
        internal const string CONF_FNAME = "Accelerometer.cfg";

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



        public static void Load(Config to)
        {
            if (!MyAPIGateway.Utilities.FileExistsInLocalStorage(CONF_FNAME, typeof(Config)))
            {
                MyAPIGateway.Utilities.WriteFileInLocalStorage(CONF_FNAME, typeof(Config)).Close();
            }
            using (var f = MyAPIGateway.Utilities.ReadFileInLocalStorage(CONF_FNAME, typeof(Config)))
            {
                to = MyAPIGateway.Utilities.SerializeFromXML<Config>(f.ReadToEnd());
            }
        }
        public void Save()
        {
            using (var f = MyAPIGateway.Utilities.WriteFileInLocalStorage(CONF_FNAME, typeof(Config)))
            {
                f.Write(MyAPIGateway.Utilities.SerializeToXML(this));
            }

        }


    }
}
