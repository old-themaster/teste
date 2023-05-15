using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossAddDamageBuffer : AbstractBuffer
	{
		public WorldBossAddDamageBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossAddDamageBuffer worldBossAddDamageBuffer = player.BufferList.GetOfType(typeof(WorldBossAddDamageBuffer)) as WorldBossAddDamageBuffer;
			if (worldBossAddDamageBuffer != null)
			{
				worldBossAddDamageBuffer.Info.ValidDate = base.Info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossAddDamageBuffer);
				player.UpdateFightBuff(base.Info);
				return;
			}
			base.Start(player);
			player.FightBuffs.Add(base.Info);
		}
		public override void Stop()
		{
			this.m_player.FightBuffs.Remove(base.Info);
			base.Stop();
		}
	}
}
