using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossAttrackBuffer : AbstractBuffer
	{
		public WorldBossAttrackBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossAttrackBuffer worldBossAttrackBuffer = player.BufferList.GetOfType(typeof(WorldBossAttrackBuffer)) as WorldBossAttrackBuffer;
			if (worldBossAttrackBuffer != null)
			{
				worldBossAttrackBuffer.Info.ValidDate = base.Info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossAttrackBuffer);
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
