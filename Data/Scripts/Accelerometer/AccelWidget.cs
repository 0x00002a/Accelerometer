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
        private HudAPIv2.HUDMessage unit_lbl_;
        private StringBuilder msg_ = new StringBuilder("");
        private StringBuilder unit_msg_ = new StringBuilder("");

        private HudAPIv2.MenuScreenInput pos_input_;
        private HudAPIv2.MenuItem gforce_sel_;
        private HudAPIv2.MenuItem metric_sel_;

        public Config Conf;

        private float last_accel_mps_ = 0.0f;

        public float CurrentAccel { set {
                if (value != last_accel_mps_)
                {
                    current_accel_ = value;
                    last_accel_mps_ = value;
                    msg_.Clear();
                    msg_.Append(current_accel_.ToString());
                    Relayout();
                }
            } }

        private void Relayout()
        {
            if (unit_lbl_ != null)
            {
                var w_offset = romiter_handle_.GetTextLength().X;
                unit_lbl_.Origin = new Vector2D(romiter_handle_.Origin.X + w_offset, romiter_handle_.Origin.Y);


                var ui_visible = current_accel_ > 0f;
                unit_lbl_.Visible = ui_visible;
                romiter_handle_.Visible = ui_visible;
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
            unit_lbl_ = new HudAPIv2.HUDMessage(unit_msg_, Origin: new Vector2D(0, 0), Blend: BlendTypeEnum.PostPP);

            unit_lbl_.Visible = false;
            unit_lbl_.Scale = 1.2;

            var menu_root = new HudAPIv2.MenuRootCategory("Accelerometer");
            pos_input_ = new HudAPIv2.MenuScreenInput(Text: "Position", Parent: menu_root, Origin: Conf.Position, Size: romiter_handle_.GetTextLength(), OnSubmit: pos =>
            {
                romiter_handle_.Origin = pos;
                Conf.Position = pos;
            });

            var force_cat = new HudAPIv2.MenuSubCategory(Text: "Unit", Parent: menu_root, HeaderText: "Unit");
            var gforce_sel = new HudAPIv2.MenuItem(Text: "GForce", Parent: force_cat, OnClick: () =>
            {
                Conf.Unit = AccelUnit.GForce;
                OnUnitChanged(AccelUnit.GForce);
            });
            var metric_sel = new HudAPIv2.MenuItem(Text: "Metric", Parent: force_cat, OnClick: () =>
            {
                Conf.Unit = AccelUnit.Metric;
                OnUnitChanged(AccelUnit.Metric);
            });

            OnUnitChanged(Conf.Unit);

        }
        
        private void OnUnitChanged(AccelUnit new_unit)
        {
            unit_msg_.Clear();

            switch(new_unit)
            {
                case AccelUnit.Metric:
                    unit_msg_.Append("m/s²");
                    break;
                case AccelUnit.GForce:
                    unit_msg_.Append("g");
                    break;
            }
        }
        public void Dispose()
        {
            hud_handle_.Close();
        }

        public void Draw()
        {
            if (romiter_handle_ != null)
            {
            }
        }
    }
}
