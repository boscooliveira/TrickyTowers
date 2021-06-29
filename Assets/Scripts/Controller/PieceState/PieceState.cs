using GameProject.TrickyTowers.Config;
using UnityEngine;

namespace GameProject.TrickyTowers.Controller.PieceState
{
    public interface IPieceState
    {
        void Activate();
        void SetSpeed(float pace);
    }

    public class PieceStateImpl : IPieceState
    {
        public readonly PiecePhysics PiecePhysics;

        private readonly Rigidbody2D _rigidBody;

        private readonly ConstantForce2D _constantForce;

        public PieceStateImpl(PiecePhysics physics, Rigidbody2D rigidBody, ConstantForce2D constantForce)
        {
            PiecePhysics = physics;
            _rigidBody = rigidBody;
            _constantForce = constantForce;
        }

        public void Activate()
        {
            _rigidBody.mass = PiecePhysics.Mass;
            _rigidBody.drag = PiecePhysics.LinearDrag;
            _rigidBody.angularDrag = PiecePhysics.AngularDrag;
            _constantForce.force = Vector2.down * PiecePhysics.GravityForce;
            _rigidBody.constraints = PiecePhysics.Constraints;
        }

        public void SetSpeed(float pace)
        {
            _rigidBody.velocity = Vector3.down * 5;
            _constantForce.force = Vector2.down * PiecePhysics.GravityForce * pace;
        }
    }
}
