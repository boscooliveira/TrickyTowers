using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IPhysicsConfig
    {
        PiecePhysics BeforePlacedPhysics { get; }
        PiecePhysics AfterPlacedPhysics { get; }
    }

    [CreateAssetMenu(fileName = "InGamePhysicsConfigSO", menuName = "TrickyTowersJoao/InGamePhysicsConfig")]
    public class InGamePhysicsConfigSO : ScriptableObject, IPhysicsConfig
    {
        [SerializeField]
        private PiecePhysics _beforePlacedPhysics;

        [SerializeField]
        private PiecePhysics _afterPlacedPhysics;

        public PiecePhysics BeforePlacedPhysics => _beforePlacedPhysics;
        public PiecePhysics AfterPlacedPhysics => _afterPlacedPhysics;
    }

    [System.Serializable]
    public class PiecePhysics
    {
        [SerializeField]
        private float _mass = 10;

        [SerializeField]
        private float _constGravityForce = 10;

        [SerializeField]
        private float _linearDrag = 5;

        [SerializeField]
        private float _angularDrag = 1;

        [SerializeField]
        private RigidbodyConstraints2D _contraints;

        public float Mass => _mass;
        public float GravityForce => _constGravityForce;
        public float LinearDrag => _linearDrag;
        public float AngularDrag => _angularDrag;
        public RigidbodyConstraints2D Constraints => _contraints;
    }
}
