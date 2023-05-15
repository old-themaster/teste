using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossAttrack_MoneyBuffBuffer : AbstractBuffer
	{
		public WorldBossAttrack_MoneyBuffBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossAttrack_MoneyBuffBuffer worldBossAttrack_MoneyBuffBuffer = player.BufferList.GetOfType(typeof(WorldBossAttrack_MoneyBuffBuffer)) as WorldBossAttrack_MoneyBuffBuffer;
			if (worldBossAttrack_MoneyBuffBuffer != null)
			{
				worldBossAttrack_MoneyBuffBuffer.Info.ValidDate = this.m_info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossAttrack_MoneyBuffBuffer);
				for (int i = 0; i < player.FightBuffs.Count; i++)
				{
					if (player.FightBuffs[i].Type == this.m_info.Type && player.FightBuffs[i].ValidCount < 20)
					{
						player.FightBuffs[i].Value += this.m_info.Value;
						player.FightBuffs[i].ValidCount++;
						return;
					}
				}
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
