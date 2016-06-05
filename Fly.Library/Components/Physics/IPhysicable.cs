// <copyright file="IPhysicable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.IPhysicable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom;
    using Atom.Math;
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Represents an <see cref="IFlyComponent"/> that adds a (FarseerPhysics-based) physics behaviour to an entity.
    /// </summary>
    public interface IPhysicable : IFlyComponent
    {
        event SimpleEventHandler<IPhysicable> Generated;

        Body Body
        {
            get;
        }

        object FixtureData
        {
            get;
        }

        float Mass
        {
            get;
        }

        Vector2 Center
        {
            get;
        }

        Vector2 LinearVelocity
        {
            get;
            set;
        }

        bool IsStatic
        {
            get;
            set;
        }

        float AngularDamping
        {
            get;
            set;
        }

        void ApplyForce( Vector2 force );
        void ApplyTorque( float torque );
    }
}
