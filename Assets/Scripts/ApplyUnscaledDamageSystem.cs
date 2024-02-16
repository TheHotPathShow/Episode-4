using Unity.Entities;

namespace THPS.DamageSystem
{
    public partial struct ApplyUnscaledDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (currentHitPoints, damageBuffer, team) in SystemAPI
                         .Query<RefRW<CurrentHitPoints>, DynamicBuffer<DamageBufferElement>, EntityTeam>()
                         .WithAll<IgnoreDamageMultiplicationTag>()
                         .WithOptions(EntityQueryOptions.FilterWriteGroup))
            {
                foreach (var damageElement in damageBuffer)
                {
                    if (damageElement.DamageType == DamageType.None) continue;
                    if (damageElement.DamageType == DamageType.Healing)
                    {
                        if (team.Value != damageElement.DamageTeam) continue;
                        currentHitPoints.ValueRW.Value += damageElement.HitPoints;
                        continue;
                    }

                    if (team.Value == damageElement.DamageTeam) continue;
                    currentHitPoints.ValueRW.Value -= damageElement.HitPoints;
                }
            }
        }
    }
}