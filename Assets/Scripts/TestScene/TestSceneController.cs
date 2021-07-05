using UnityEngine;
using GameProject.TrickyTowers.Model;
using GameProject.TrickyTowers.Engine;
using GameProject.TrickyTowers.Config;
using GameProject.TrickyTowers.Service;
using GameProject.TrickyTowers.Controller;
using System.Collections;
using System.Collections.Generic;

namespace GameProject.TrickyTowers.TestScene
{
    public class TestSceneController : MonoBehaviour, IUnityProxy, IGameConfigService, IPhysicsConfig
    {
        public PlayerController PlayerController;
        public GameplayConfigSO GameplayConfigSO;
        public InGamePhysicsConfigSO InGamePhysicsConfigSO;
        public PieceFactoryConfigSO PieceFactoryConfigSO;
        public EPlayerInputType SimulationInput;

        public float HorizontalMoveDistance { get; set; }

        public float SlowPace { get; set; }

        public float FastPace { get; set; }

        public PhysicsMaterial2D _physicsMaterial2D;
        public PhysicsMaterial2D PhysicsMaterial2D => _physicsMaterial2D;

        public List<PieceConfig> Pieces { get; set; }

        public PiecePhysics BeforePlacedPhysics => InGamePhysicsConfigSO.BeforePlacedPhysics;

        public PiecePhysics AfterPlacedPhysics => InGamePhysicsConfigSO.AfterPlacedPhysics;

        public IPieceFactoryConfig PieceConfig { get; set; }

        public IPhysicsConfig PhysicsConfig { get; set; }

        public IGameplayConfig GameplayConfig => GameplayConfigSO;

        private IGameplayService _gameplay;

        private void Awake()
        {
            LoadData();
            RegisterServices();
        }

        public void LoadData()
        {
            PieceConfig = new TestFactoryConfig(PieceFactoryConfigSO);
        }

        public void UpdatePositions(float goalHeight, float spawnerHeight)
        {
            var bottom = PlayerController.Bounds.LimitBottom.position;
            
            var position = PlayerController.Bounds.Goal.position;
            position.y = bottom.y + goalHeight;
            PlayerController.Bounds.Goal.position = position;

            position = PlayerController.Bounds.SpawnerPosition.position;
            position.y = bottom.y + spawnerHeight;
            PlayerController.Bounds.SpawnerPosition.position = position;
        }

        private void RegisterServices()
        {
            ServiceFactory.Instance.Register<IUnityProxy>(this);
            ServiceFactory.Instance.Register<IGameConfigService>(this);
            _gameplay = new GameplayService(this);
            ServiceFactory.Instance.Register<IGameplayService>(_gameplay);

            if (SimulationInput == EPlayerInputType.RandomTargetAI)
            {
                GetComponent<ForceSceneView>().enabled = true;
            }

            _gameplay.StartNewGame(false);
            PlayerController.Initialize(PieceConfig, this, _gameplay, new TestPlayer(SimulationInput), OnGameOver);
            PlayerController.gameObject.SetActive(true);
        }

        public void ExecuteCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void EnterGameScene() { }
        public void EnterMenuScene() { }
        private void OnGameOver(IPlayerData player, PlayerController controller) { }
    }
}