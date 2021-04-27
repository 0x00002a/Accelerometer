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
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum; // required for MyTransparentGeometry/MySimpleObjectDraw to be able to set blend type.

using Draygo.API;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using Digi;

namespace Natomic.Accelerometer
{
    // This object is always present, from the world load to world unload.
    // NOTE: all clients and server run mod scripts, keep that in mind.
    // NOTE: this and gamelogic comp's update methods run on the main game thread, don't do too much in a tick or you'll lower sim speed.
    // NOTE: also mind allocations, avoid realtime allocations, re-use collections/ref-objects (except value types like structs, integers, etc).
    //
    // The MyUpdateOrder arg determines what update overrides are actually called.
    // Remove any method that you don't need, none of them are required, they're only there to show what you can use.
    // Also remove all comments you've read to avoid the overload of comments that is this file.
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class Session: MySessionComponentBase
    {
        public static Session Instance; // the only way to access session comp from other classes and the only accepted static field.
        private AccelWidget hud_display_;
        private ulong tick = 0;

        public override void LoadData()
        {
            Instance = this;

            if (!MyAPIGateway.Utilities.IsDedicated)
            {
                hud_display_ = new AccelWidget();
                hud_display_.Conf = Config.Load();
                hud_display_.Init();
            } else
            {
                UpdateOrder = MyUpdateOrder.NoUpdate;
            }

        }
        


        protected override void UnloadData()
        {
            Instance = null; // important for avoiding this object to remain allocated in memory

            if (hud_display_ != null)
            {
                hud_display_.Conf.Save();
                hud_display_.Dispose();
            }
        }


        private float CalcAccel(IMyCharacter character)
        {
            var parent = character.Parent;

            var physics = character.Physics;
            var com = character.WorldAABB.Center;
            var worldPos = character.GetPosition();
           
            if (parent != null)
            {
                var parentCube = (parent as IMyCubeBlock)?.CubeGrid;
                if (parentCube != null)
                {
                    physics = parentCube.Physics;
                    com = parentCube.Physics.CenterOfMassWorld;
                    worldPos = parentCube.GetPosition();
                }

            }
            return physics != null ? (physics.LinearAcceleration + physics.AngularAcceleration.Cross(worldPos - com)).Length() : 0;
        }

        public override void UpdateAfterSimulation()
        {
            ++tick;

            try
            {
                if (tick % 10 == 0)
                {
                    var player = MyAPIGateway.Session.Player;
                    if (player != null && player.Character != null)
                    {
                        hud_display_.CurrentAccel = CalcAccel(player.Character);
                    }
                }
            }
            catch(Exception e) 
            {
                Log.Error(e, e.Message);
            }
        }


        public override void SaveData()
        {
            // executed AFTER world was saved
            hud_display_?.Conf.Save();
        }
        
    }
}