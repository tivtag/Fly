// <copyright file="GravityEmitting.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.GravityEmitting class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom.Math;
    using Fly.Entities;

    /// <summary>
    /// Adds the mechanism of being able to emit the force of gravity to the entity.
    /// </summary>
    public sealed class GravityEmitting : FlyComponent
    {        
        public float GravityFactor 
        {
            get;
            set;
        }

        public override void InitializeBindings()
        {
            this.emittingPhysics = this.Owner.Components.Find<IPhysicable>();
        }

        public void ApplyGravityTo( IGravityReceivingEntity entity )
        {
            GravityReceiving gravity = entity.GravityReceiver;

            if( gravity.IsEnabled )
            {
                IPhysicable receivingPhysics = entity.Physics;

                Vector2 delta = this.emittingPhysics.Center - receivingPhysics.Center;
                float distance = delta.SquaredLength;

                if( distance != 0.0f )
                {
                    float strength = CalculateGravityStrength( distance, gravity, receivingPhysics );
                    Vector2 force = delta.Direction * strength;

                    receivingPhysics.ApplyForce( force );
                }
            }
        }

        private float CalculateGravityStrength( float squaredDistanceBetweenEmitterAndReceiver, GravityReceiving gravityReceiving, IPhysicable receivingPhysics )
        {
            float factor = gravityReceiving.Factor * 0.001f;
            float strength = factor * ((this.emittingPhysics.Mass * receivingPhysics.Mass) / squaredDistanceBetweenEmitterAndReceiver);
            return strength;
        }

        private IPhysicable emittingPhysics;
    }
}
