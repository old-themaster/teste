using SqlDataProvider.Data;
namespace Game.Server.Buffer
{
    public class WorldBossHP_MoneyBuffBuffer : AbstractBuffer
	{
		public WorldBossHP_MoneyBuffBuffer(BufferInfo buffer) : base(buffer)
		{
		}
		public override void Start(GamePlayer player)
		{
			WorldBossHP_MoneyBuffBuffer worldBossHP_MoneyBuffBuffer = player.BufferList.GetOfType(typeof(WorldBossHP_MoneyBuffBuffer)) as WorldBossHP_MoneyBuffBuffer;
			if (worldBossHP_MoneyBuffBuffer != null)
			{
				worldBossHP_MoneyBuffBuffer.Info.ValidDate = this.m_info.ValidDate;
				player.BufferList.UpdateBuffer(worldBossHP_MoneyBuffBuffer);
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
