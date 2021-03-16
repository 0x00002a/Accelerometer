﻿/* 
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
        private AccelWidget hud_display_ = new AccelWidget();
        private int tick = 0;

        public override void LoadData()
        {
            // amogst the earliest execution points, but not everything is available at this point.

            // These can be used anywhere, not just in this method/class:
            // MyAPIGateway. - main entry point for the API
            // MyDefinitionManager.Static. - reading/editing definitions
            // MyGamePruningStructure. - fast way of finding entities in an area
            // MyTransparentGeometry. and MySimpleObjectDraw. - to draw sprites (from TransparentMaterials.sbc) in world (they usually live a single tick)
            // MyVisualScriptLogicProvider. - mainly designed for VST but has its uses, use as a last resort.
            // System.Diagnostics.Stopwatch - for measuring code execution time.
            // ...and many more things, ask in #programming-modding in keen's discord for what you want to do to be pointed at the available things to use.

            Instance = this;

            hud_display_.Init();
        }
        

        public override void BeforeStart()
        {
            // executed before the world starts updating
        }

        protected override void UnloadData()
        {
            // executed when world is exited to unregister events and stuff

            Instance = null; // important for avoiding this object to remain allocated in memory
        }

        public override void HandleInput()
        {
            // gets called 60 times a second before all other update methods, regardless of framerate, game pause or MyUpdateOrder.
        }

        public override void UpdateBeforeSimulation()
        {
            // executed every tick, 60 times a second, before physics simulation and only if game is not paused.
        }

        public override void Simulate()
        {
            // executed every tick, 60 times a second, during physics simulation and only if game is not paused.
            // NOTE in this example this won't actually be called because of the lack of MyUpdateOrder.Simulation argument in MySessionComponentDescriptor
        }
        private void UpdatePlayerCache()
        {

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
            // executed every tick, 60 times a second, after physics simulation and only if game is not paused.
            ++tick;

            try // example try-catch for catching errors and notifying player, use only for non-critical code!
            {
                if (tick % 10 == 0)
                {
                    var player = MyAPIGateway.Session.Player;
                    if (player != null)
                    {
                        hud_display_.CurrentAccel = (float)Math.Round(CalcAccel(player.Character), 2);
                    }
                }
            }
            catch(Exception e) // NOTE: never use try-catch for code flow or to ignore errors! catching has a noticeable performance impact.
            {
                Log.Error(e, e.Message);

            }
        }

        public override void Draw()
        {
            // gets called 60 times a second after all other update methods, regardless of framerate, game pause or MyUpdateOrder.
            // NOTE: this is the only place where the camera matrix (MyAPIGateway.Session.Camera.WorldMatrix) is accurate, everywhere else it's 1 frame behind.
            try
            {
                if (hud_display_ != null)
                {
                    hud_display_.Draw();
                }
            } catch(Exception e)
            {
                Log.Error($"Failed to draw: {e.Message}", e.Message);

            }
        }

        public override void SaveData()
        {
            // executed AFTER world was saved
        }

        public override MyObjectBuilder_SessionComponent GetObjectBuilder()
        {
            // executed during world save, most likely before entities.

            return base.GetObjectBuilder(); // leave as-is.
        }

        public override void UpdatingStopped()
        {
            // executed when game is paused
        }
    }
}