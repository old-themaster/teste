using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossHPBuffer : AbstractBuffer
	{
		public WorldBossHPBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossHPBuffer worldBossHPBuffer = player.BufferList.GetOfType(typeof(WorldBossHPBuffer)) as WorldBossHPBuffer;
			if (worldBossHPBuffer != null)
			{
				worldBossHPBuffer.Info.ValidDate = base.Info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossHPBuffer);
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
