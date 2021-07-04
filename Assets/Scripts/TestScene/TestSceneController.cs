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
    [RequireComponent(typeof(GameBoundsGizmos))]
    public class TestSceneController : MonoBehaviour, IUnityProxy, IGameConfigService, IPhysicsConfig, IGameplayConfig
    {
        public PlayerController PlayerController;
        public GameplayConfigSO GameplayConfigSO;
        public InGamePhysicsConfigSO InGamePhysicsConfigSO;
        public PieceFactoryConfigSO PieceFactoryConfigSO;

        public float HorizontalMoveDistance { get; set; }

        public float SlowPace { get; set; }

        public float FastPace { get; set; }

        public PhysicsMaterial2D _physicsMaterial2D;
        public PhysicsMaterial2D PhysicsMaterial2D => _physicsMaterial2D;

        public List<PieceConfig> Pieces { get; set; }

        public PiecePhysics BeforePlacedPhysics { get; set; }

        public PiecePhysics AfterPlacedPhysics { get; set; }

        public int InitialLives { get; set; }

        public float GhostSecsAfterHit { get; set; }

        public float GoalHeight { get; set; }

        public IPieceFactoryConfig PieceConfig { get; set; }

        public IPhysicsConfig PhysicsConfig { get; set; }

        public IGameplayConfig GameplayConfig => this;

        public float SpawnerMinDistance { get; set; }

        private IGameplayService _gameplay;

        private void Awake()
        {
            LoadData();
            RegisterServices();
        }

        private void LoadData()
        {
            InitialLives = GameplayConfigSO.InitialLives;
            GhostSecsAfterHit = GameplayConfigSO.GhostSecsAfterHit;
            GoalHeight = GameplayConfigSO.GoalHeight;
            SpawnerMinDistance = GameplayConfigSO.SpawnerMinDistance;

            BeforePlacedPhysics = InGamePhysicsConfigSO.BeforePlacedPhysics;
            AfterPlacedPhysics = InGamePhysicsConfigSO.AfterPlacedPhysics;
            PieceConfig = new TestFactoryConfig(PieceFactoryConfigSO);
        }

        private void SaveData()
        {
            GameplayConfigSO.SetValues(InitialLives, GhostSecsAfterHit, GoalHeight, SpawnerMinDistance);

            InGamePhysicsConfigSO.BeforePlacedPhysics.CopyValues(BeforePlacedPhysics);
            InGamePhysicsConfigSO.AfterPlacedPhysics.CopyValues(AfterPlacedPhysics);
        }

        private void RegisterServices()
        {
            ServiceFactory.Instance.Register<IUnityProxy>(this);
            ServiceFactory.Instance.Register<IGameConfigService>(this);
            _gameplay = new GameplayService(this);
            ServiceFactory.Instance.Register<IGameplayService>(_gameplay);

            _gameplay.StartNewGame(false);
            PlayerController.Initialize(PieceConfig, this, _gameplay, new TestPlayer(), OnGameOver);
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