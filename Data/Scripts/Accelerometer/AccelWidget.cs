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
using Digi;
using Draygo.API;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Utils;
using VRageMath;

using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;
namespace Natomic.Accelerometer
{

    /// <summary>
    /// Contains functions to transform the 1, -1 -> -1, 1 coordinate system into a sane 0, 0 -> 1, 1 
    /// </summary>
    static class CoordHelper
    {

        public static Vector2D SaneToKeenCoord(Vector2D sane)
        {
            var scaled = sane * 2;
            return new Vector2D(-1 * (1 - scaled.X), 1 - scaled.Y);
        } 
        public static Vector2D KeenCoordToSane(Vector2D keen)
        {
            // keen.kill()
            return new Vector2D(1 + (keen.X / 2 - 0.5), 1 - (keen.Y / 2 + 0.5));
        }
    }
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
            hud_handle_ = new HudAPIv2(OnHUDRegistered);
        }

        private void ChangePos(Vector2D pos)
        {
            var sane_pos = CoordHelper.KeenCoordToSane(pos);

            var bounds = CoordHelper.KeenCoordToSane(romiter_handle_.GetTextLength());
            var txt_pos = new Vector2D(sane_pos.X + bounds.X / 2, sane_pos.Y + bounds.Y / 2);
            romiter_handle_.Origin = CoordHelper.SaneToKeenCoord(txt_pos);

            Conf.Position = pos;
            pos_input_.Origin = pos;
        }

        private void OnHUDRegistered()
        {
            try
            {
                romiter_handle_ = new HudAPIv2.HUDMessage(msg_, Conf.Position, null, -1, 1.2, true, false, null, BlendTypeEnum.PostPP);


                menu_ = new HudAPIv2.MenuRootCategory("Accelerometer", AttachedMenu: HudAPIv2.MenuRootCategory.MenuFlag.PlayerMenu, HeaderText: "Config");
                pos_input_ = new HudAPIv2.MenuScreenInput(Text: "Position", Parent: menu_, Origin: Conf.Position, Size: romiter_handle_.GetTextLength(), OnSubmit: ChangePos
                , InputDialogTitle: "X");


                var force_cat = new HudAPIv2.MenuSubCategory(Text: "Unit", Parent: menu_, HeaderText: "Unit");
                gforce_sel_ = new HudAPIv2.MenuItem(Text: "GForce", Parent: force_cat, OnClick: () =>
                {
                    Conf.Unit = AccelUnit.GForce;
                });
                metric_sel_ = new HudAPIv2.MenuItem(Text: "Metric", Parent: force_cat, OnClick: () =>
                {
                    Conf.Unit = AccelUnit.Metric;
                });
            } catch(Exception e)
            {
                Log.Error(e, $"Error in hud register: {e.Message}");
            }

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

    }
}
