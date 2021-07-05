using UnityEngine;
using System.Collections.Generic;

namespace GameProject.TrickyTowers.Controller
{
    public class GoalController : MonoBehaviour
    {
        private HashSet<Collider2D> _colliders = new HashSet<Collider2D>();

        public bool IsColliding()
        {
            return _colliders.Count > 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _colliders.Add(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _colliders.Remove(other);
        }
    }
}
