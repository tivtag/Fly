
namespace Fly.Physics.Descriptors
{
    using FarseerPhysics.Dynamics;
    using Fly.Saving;

    public abstract class BasePhysicsBodyDescription : IPhysicsBodyDescription, ISaveable
    {
        public bool IsStatic { get; set; }

        public float Density { get; set; }

        public abstract Body Build( World world );

        protected virtual Body BaseInit( Body body )
        {
            if( !this.IsStatic )
            {
                body.BodyType = BodyType.Dynamic;
            }

            body.IsStatic = this.IsStatic;
            return body;
        }

        public virtual void Serialize( Atom.Storage.ISerializationContext context )
        {
            throw new System.NotImplementedException( "Serialization is not implemented for type " + this.GetType() );
        }

        public virtual void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            throw new System.NotImplementedException( "Deserialization is not implemented for type " + this.GetType() );
        }
    }
}
