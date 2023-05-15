using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossMetalSlugBuffer : AbstractBuffer
	{
		public WorldBossMetalSlugBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossMetalSlugBuffer worldBossMetalSlugBuffer = player.BufferList.GetOfType(typeof(WorldBossMetalSlugBuffer)) as WorldBossMetalSlugBuffer;
			if (worldBossMetalSlugBuffer != null)
			{
				worldBossMetalSlugBuffer.Info.ValidDate = base.Info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossMetalSlugBuffer);
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
