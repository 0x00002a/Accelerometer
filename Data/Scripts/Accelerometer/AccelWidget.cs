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
using VRage.Utils;
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

        private HudAPIv2.MenuScreenInput pos_input_;
        private HudAPIv2.MenuItem gforce_sel_;
        private HudAPIv2.MenuItem metric_sel_;
        private HudAPIv2.MenuRootCategory menu_;

        public Config Conf;

        private float last_accel_mps_ = 0.0f;

        public float CurrentAccel { set {
                if (value != last_accel_mps_)
                {
                    current_accel_ = value;
                    last_accel_mps_ = value;
                    Relayout();
                }
            } }

        private float ApplyUnitScaling(float to, AccelUnit unit)
        {
            if (unit == AccelUnit.GForce)
            {
                return to / 9.8f;
            } else
            {
                return to;
            }
        }
        private void Relayout()
        {
            if (romiter_handle_ != null)
            {
                msg_.Clear();

                var ui_visible = current_accel_ > 0f;
                romiter_handle_.Visible = ui_visible;

                var accel = (float)Math.Round(ApplyUnitScaling(current_accel_, Conf.Unit), 2);
                msg_.Append($"{accel} ");
                AppendUnit(msg_, Conf.Unit);
            }
        }

        public void Init()
        {
            if (!(MyAPIGateway.Utilities.IsDedicated || (MyAPIGateway.Multiplayer.MultiplayerActive && MyAPIGateway.Multiplayer.IsServer)))
            {
                hud_handle_ = new HudAPIv2(OnHUDRegistered);
            }
        }

        private void OnHUDRegistered()
        {
            romiter_handle_ = new HudAPIv2.HUDMessage(msg_, Conf.Position, null, -1, 1.2, true, false, null, BlendTypeEnum.PostPP);


            menu_ = new HudAPIv2.MenuRootCategory("Accelerometer", AttachedMenu: HudAPIv2.MenuRootCategory.MenuFlag.PlayerMenu, HeaderText: "Config");
            pos_input_ = new HudAPIv2.MenuScreenInput(Text: "Position", Parent: menu_, Origin: Conf.Position, Size: romiter_handle_.GetTextLength(), OnSubmit: pos =>
            {
                romiter_handle_.Origin = pos;
                Conf.Position = pos;
                pos_input_.Origin = pos;
            }, InputDialogTitle: "[H]");
            

            var force_cat = new HudAPIv2.MenuSubCategory(Text: "Unit", Parent: menu_, HeaderText: "Unit");
            gforce_sel_ = new HudAPIv2.MenuItem(Text: "GForce", Parent: force_cat, OnClick: () =>
            {
                Conf.Unit = AccelUnit.GForce;
            });
            metric_sel_ = new HudAPIv2.MenuItem(Text: "Metric", Parent: force_cat, OnClick: () =>
            {
                Conf.Unit = AccelUnit.Metric;
            });


        }
        
        private void AppendUnit(StringBuilder to, AccelUnit unit)
        {
            switch(unit)
            {
                case AccelUnit.Metric:
                    to.Append("m/s²");
                    break;
                case AccelUnit.GForce:
                    to.Append("g");
                    break;
            }

        }
        public void Dispose()
        {
            hud_handle_.Close();
        }

        public void Draw()
        {
        }
    }
}
