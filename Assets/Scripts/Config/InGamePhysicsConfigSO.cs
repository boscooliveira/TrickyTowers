using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IPhysicsConfig
    {
        float GravityForce { get; }
        float LinearDrag { get; }
        float AngularDrag { get; }
    }

    [CreateAssetMenu(fileName = "InGamePhysicsConfigSO", menuName = "TrickyTowersJoao/InGamePhysicsConfig")]
    public class InGamePhysicsConfigSO : ScriptableObject, IPhysicsConfig
    {
        [SerializeField]
        private float _gravityForce = 10;
        [SerializeField]
        private float _linearDrag = 5;
        [SerializeField]
        private float _angularDrag = 1;

        public float GravityForce => _gravityForce;
        public float LinearDrag => _linearDrag;
        public float AngularDrag => _angularDrag;
    }
}
