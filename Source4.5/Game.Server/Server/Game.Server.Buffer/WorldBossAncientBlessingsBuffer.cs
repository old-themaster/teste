using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossAncientBlessingsBuffer : AbstractBuffer
	{
		public WorldBossAncientBlessingsBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossAncientBlessingsBuffer worldBossAncientBlessingsBuffer = player.BufferList.GetOfType(typeof(WorldBossAncientBlessingsBuffer)) as WorldBossAncientBlessingsBuffer;
			if (worldBossAncientBlessingsBuffer != null)
			{
				worldBossAncientBlessingsBuffer.Info.ValidDate = base.Info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossAncientBlessingsBuffer);
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
