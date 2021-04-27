/* 
Accelerometer Space Engineers mod
Copyright (C) 2021 Natasha England-Elbro

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

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

        [DefaultValue(true)]
        public bool Autohide;

        // C# is very limited when it comes to user-defined types apparently 
        internal double position_x_ = -0.64;

        internal double position_y_ = -0.64;

        public Vector2D Position
        {
            set { position_x_ = value.X; position_y_ = value.Y; }
            get { return new Vector2D(position_x_, position_y_); }
        }



        public static Config Load()
        {
            if (!MyAPIGateway.Utilities.FileExistsInLocalStorage(CONF_FNAME, typeof(Config)))
            {
                MyAPIGateway.Utilities.WriteFileInLocalStorage(CONF_FNAME, typeof(Config)).Close();
            }
            using (var f = MyAPIGateway.Utilities.ReadFileInLocalStorage(CONF_FNAME, typeof(Config)))
            {
                var rs = MyAPIGateway.Utilities.SerializeFromXML<Config>(f.ReadToEnd());
                if (rs == null)
                {
                    // Failed to parse 
                    rs = new Config
                    {
                        Unit = AccelUnit.Metric,
                        Autohide = true,
                    };
                }
                return rs;
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
