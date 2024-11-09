using UnityEngine;
//using UnityEngine.AI;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Chase State", menuName = "Winter Universe/Character/NPC/State/New Chase State")]
    public class ChaseState : NPCState
    {
        public override NPCState Tick(NPCController npc)
        {
            if (npc.IsPerfomingAction)
            {
                return this;
            }
            if (npc.PawnCombat.CurrentTarget == null || npc.PawnCombat.CurrentTarget.IsDead)
            {
                return SwitchState(npc, npc.IdleState);
            }
            if (npc.PawnCombat.DistanceToTarget <= npc.CombatPhase.CurrentPhase.State.MaxCombatRadius)
            {
                return SwitchState(npc, npc.CombatPhase.CurrentPhase.State);
            }
            npc.Agent.SetDestination(npc.PawnCombat.CurrentTarget.transform.position);
            //NavMeshPath path = new();
            //npc.Agent.CalculatePath(npc.NPCCombatManager.CurrentTarget.transform.position, path);
            //npc.Agent.SetPath(path);
            return this;
        }
    }
}