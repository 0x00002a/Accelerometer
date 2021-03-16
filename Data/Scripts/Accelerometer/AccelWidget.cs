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
using System;
using System.Collections.Generic;
using System.Text;

using Draygo.API;
using Sandbox.ModAPI;
using VRageMath;

using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;
namespace Natomic.Accelerometer
{
    class AccelWidget
    {
        private HudAPIv2 hud_handle_;
        private float current_accel_ = 0.0f;
        private HudAPIv2.HUDMessage romiter_handle_;
        private StringBuilder msg_ = new StringBuilder("");

        public float CurrentAccel { set { 
                current_accel_ = value; 
                msg_.Clear();
                msg_.Append(current_accel_.ToString());
                msg_.Append(" m/s^2");
            } }


        public void Init()
        {
            if (!MyAPIGateway.Utilities.IsDedicated)
            {
                hud_handle_ = new HudAPIv2(OnHUDRegistered);
            }
        }

        private void OnHUDRegistered()
        {
            romiter_handle_ = new HudAPIv2.HUDMessage(msg_, new Vector2D(-0.68, -0.64), null, -1, 1.2, true,false, null, BlendTypeEnum.PostPP);

        }

        public void Draw()
        {
            if (romiter_handle_ != null)
            {
                romiter_handle_.Visible = current_accel_ > 0f; 
                romiter_handle_.Draw();
            }
        }
    }
}
