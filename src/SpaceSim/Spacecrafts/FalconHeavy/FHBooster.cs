﻿using SpaceSim.Engines;
using SpaceSim.Physics;
using SpaceSim.Spacecrafts.FalconCommon;
using VectorMath;

using System;
using System.Drawing;
using SpaceSim.Drawing;

namespace SpaceSim.Spacecrafts.FalconHeavy
{
    sealed class FHBooster : F9S1Base
    {
        public override string CraftName { get { return "FH Booster " + Id; } }

        public override string CommandFileName
        {
            get { return Id == 1 ? "FHLeftBooster.xml" : "FHRightBooster.xml"; }
        }

        public int Id { get; private set; }

        public override double DryMass { get { return 23600; } }

        public override double Width { get { return 4.11; } }
        public override double Height { get { return 44.6; } }

        public new AeroDynamicProperties GetAeroDynamicProperties { get { return AeroDynamicProperties.ExtendsCrossSection; } }

        public FHBooster(string craftDirectory, int id, DVector2 position, DVector2 velocity, double propellantMass = 409272)
            : base(craftDirectory, position, velocity, 5, propellantMass, "Falcon/Heavy/booster" + id + ".png", -19.1)
        {
            Id = id;

            StageOffset = Id == 1 ? new DVector2(-4, 1.5) : new DVector2(4, 1.5);

            Engines = new IEngine[9];

            for (int i = 0; i < 9; i++)
            {
                double engineOffsetX = (i - 4.0) / 4.0;

                var offset = new DVector2(engineOffsetX * Width * 0.3, Height * 0.45);

                Engines[i] = new Merlin1D(i, this, offset);
            }
        }
        
        protected override void RenderShip(Graphics graphics, Camera camera, RectangleF screenBounds)
        {
            // set the booster offsets according to the roll
            float rollFactor = (float)Math.Cos(Roll);
            StageOffset = Id == 1 ? new DVector2(-4 * rollFactor, 1.5) : new DVector2(4 * rollFactor, 1.5);

            base.RenderShip(graphics, camera, screenBounds);
        }
    }
}
