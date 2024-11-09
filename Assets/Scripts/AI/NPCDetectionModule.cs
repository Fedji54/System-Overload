using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WinterUniverse
{
    public class NPCDetectionModule : MonoBehaviour
    {
        private NPCController _owner;
        private List<PawnController> _visibleEnemies = new();
        private List<PawnController> _visibleNeutrals = new();
        private List<PawnController> _visibleAllies = new();

        private void Awake()
        {
            _owner = GetComponent<NPCController>();
        }

        public void FindTargetInViewRange()
        {
            if (_owner.CombatModule.CurrentTarget != null || !_owner.CanTargeting)
            {
                return;
            }
            _visibleEnemies.Clear();
            _visibleNeutrals.Clear();
            _visibleAllies.Clear();
            Collider[] colliders = Physics.OverlapSphere(_owner.CombatModule.HeadPoint.position, _owner.CombatModule.ViewDistance, GameManager.StaticInstance.WorldLayer.CharacterMask);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out PawnController character) && character != _owner && !character.IsDead)
                {
                    if (Vector3.Distance(transform.position, character.transform.position) <= _owner.CombatModule.HearRadius || _owner.CombatModule.TargetIsVisible(character))
                    {
                        switch (_owner.Faction.GetState(character.Faction))
                        {
                            case RelationshipState.Enemy:
                                _visibleEnemies.Add(character);
                                break;
                            case RelationshipState.Neutral:
                                _visibleNeutrals.Add(character);
                                break;
                            case RelationshipState.Ally:
                                _visibleAllies.Add(character);
                                break;
                        }
                    }
                }
            }
            if (_visibleEnemies.Count > 0)
            {
                _owner.CombatModule.SetTarget(GetClosestEnemy());
            }
        }

        public PawnController GetClosestEnemy()
        {
            return _visibleEnemies.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault();
        }
    }
}