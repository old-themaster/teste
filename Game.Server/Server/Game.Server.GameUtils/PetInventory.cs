using Bussiness;
using Bussiness.Managers;
using Game.Logic;
using log4net;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.GameUtils
{
	public class PetInventory : PetAbstractInventory
	{
		private static readonly ILog ilog_1;

		private bool bool_0;

		private List<UsersPetInfo> list_0;

		protected GamePlayer m_player;

		private PlayerInventory playerInventory_0;

		public GamePlayer Player => m_player;

		private EatPetsInfo m_eatPets;

		public EatPetsInfo EatPets
		{
			get { return m_eatPets; }
		}


		public int MaxLevel => Convert.ToInt32(PetMgr.FindConfig("MaxLevel").Value);

		public int MaxLevelByGrade
		{
			get
			{
				if (m_player == null || m_player.Level > MaxLevel)
				{
					return MaxLevel;
				}
				return m_player.Level;
			}
		}

		public PlayerInventory Equips => playerInventory_0;

		public PetInventory(GamePlayer player, bool saveTodb, int capibility, int aCapability, int beginSlot)
			: base(capibility, aCapability, beginSlot)
		{
			m_player = player;
			bool_0 = saveTodb;
			list_0 = new List<UsersPetInfo>();
			playerInventory_0 = new PlayerInventory(player, saveTodb: true, 49, 5012, 0, autoStack: false);
		}

		internal void AddAdoptPetTo(UsersPetInfo current, int place)
		{
			throw new NotImplementedException();
		}

		public virtual void ClearAdoptPets()
		{
			using (PlayerBussiness pb = new PlayerBussiness())
			{
				lock (m_lock)
				{
					for (int i = 0; i < ACapalility; i++)
					{
						if (m_adoptPets[i] != null && m_adoptPets[i].ID > 0)
							pb.ClearAdoptPet(m_adoptPets[i].ID);

						m_adoptPets[i] = null;
					}
				}
			}
		}

		public virtual void LoadFromDatabase()
		{
			if (bool_0)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					int iD = m_player.PlayerCharacter.ID;
					UsersPetInfo[] userPetSingles = playerBussiness.GetUserPetSingles(iD);
					Equips.LoadFromDatabase();
					BeginChanges();
					try
					{
						UsersPetInfo[] array = userPetSingles;
						foreach (UsersPetInfo usersPetInfo in array)
						{
							if (string.IsNullOrEmpty(usersPetInfo.BaseProp))
							{
								string text = usersPetInfo.TemplateID.ToString();
								PetTemplateInfo petTemplateInfo = PetMgr.FindPetTemplate((text.Substring(text.Length - 1, 1) == "1") ? ((usersPetInfo.Level < 30) ? usersPetInfo.TemplateID : ((usersPetInfo.Level >= 50) ? (usersPetInfo.TemplateID - 2) : (usersPetInfo.TemplateID - 1))) : ((!(text.Substring(text.Length - 1, 1) == "2")) ? (usersPetInfo.TemplateID - 2) : (usersPetInfo.TemplateID - 1)));
								if (petTemplateInfo != null)
								{
									UsersPetInfo usersPetInfo2 = PetMgr.CreatePet(petTemplateInfo, usersPetInfo.UserID, usersPetInfo.Place, usersPetInfo.Level);
									usersPetInfo.BaseProp = JsonConvert.SerializeObject(usersPetInfo2);
									UpdateEvolutionPet(usersPetInfo2, usersPetInfo.Level, MaxLevelByGrade);
									usersPetInfo.AttackGrow = usersPetInfo2.AttackGrow;
									usersPetInfo.DefenceGrow = usersPetInfo2.DefenceGrow;
									usersPetInfo.AgilityGrow = usersPetInfo2.AgilityGrow;
									usersPetInfo.LuckGrow = usersPetInfo2.LuckGrow;
									usersPetInfo.BloodGrow = usersPetInfo2.BloodGrow;
									usersPetInfo.DamageGrow = usersPetInfo2.DamageGrow;
									usersPetInfo.GuardGrow = usersPetInfo2.GuardGrow;
								}
							}
							AddPetTo(usersPetInfo, usersPetInfo.Place);
						}
					}
					finally
					{
						try
						{
							if (FindFirstEmptySlot(base.BeginSlot) != -1 && userPetSingles.Length != 0)
							{
								for (int j = 1; FindFirstEmptySlot(base.BeginSlot) < userPetSingles[userPetSingles.Length - j].Place; j++)
								{
									Player.PetBag.MovePet(userPetSingles[userPetSingles.Length - j].Place, FindFirstEmptySlot(base.BeginSlot));
								}
							}
						}
						catch
						{
						}
						CommitChanges();
						for (int k = 0; k < Equips.Capalility; k++)
						{
							ItemInfo itemAt = Equips.GetItemAt(k);
							if (itemAt != null)
							{
								m_player.AddTemplate(ItemInfo.CloneFromTemplate(itemAt.Template, itemAt));
								Equips.RemoveItemAt(k);
							}
						}
					}
				}
			}
		}

		public virtual void SaveToDatabase(bool saveAdopt)
		{
			if (bool_0)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					lock (m_lock)
					{
						for (int i = 0; i < m_pets.Length; i++)
						{
							UsersPetInfo usersPetInfo = m_pets[i];
							if (usersPetInfo != null && usersPetInfo.IsDirty)
							{
								usersPetInfo.eQPets = SerializePetEquip(usersPetInfo.PetEquips);
								if (usersPetInfo.ID > 0)
								{
									playerBussiness.UpdateUserPet(usersPetInfo);
								}
								else
								{
									playerBussiness.AddUserPet(usersPetInfo);
								}
							}
						}
					}
					lock (m_lock)
					{
						foreach (UsersPetInfo item in list_0)
						{
							playerBussiness.UpdateUserPet(item);
						}
						list_0.Clear();
					}
				}
			}
			Equips.SaveToDatabase();
		}

		public override bool AddPetTo(UsersPetInfo pet, int place)
		{
			if (!base.AddPetTo(pet, place))
			{
				return false;
			}
			pet.UserID = m_player.PlayerCharacter.ID;
			pet.PetEquips = DeserializePetEquip(pet.eQPets);
			return true;
		}

		public virtual void ReduceHunger()
		{
			UsersPetInfo petIsEquip = GetPetIsEquip();
			if (petIsEquip == null)
			{
				return;
			}
			int num = 40;
			int num2 = 100;
			if (petIsEquip.Hunger >= 100)
			{
				if (petIsEquip.Level >= 60)
				{
					petIsEquip.Hunger -= num2;
				}
				else
				{
					petIsEquip.Hunger -= num;
				}
				UpdatePet(petIsEquip, petIsEquip.Place);
			}
		}

		public bool CanAdd(ItemInfo item, List<PetEquipInfo> infos)
		{
			if (infos.Count == 3 || item.Template == null)
			{
				return false;
			}
			foreach (PetEquipInfo info in infos)
			{
				if (item.eqType() == info.eqType)
				{
					return false;
				}
			}
			return true;
		}

		public bool AddEqPet(int place, ItemInfo item)
		{
			UsersPetInfo petAt = GetPetAt(place);
			if (petAt == null || !CanAdd(item, m_pets[place].PetEquips))
			{
				return false;
			}
			petAt.PetEquips.Add(new PetEquipInfo(item.Template)
			{
				eqTemplateID = item.TemplateID,
				eqType = item.eqType(),
				ValidDate = item.ValidDate,
				startTime = item.BeginDate
			});
			return true;
		}

		public PetEquipInfo GetEqPet(List<PetEquipInfo> infos, int place)
		{
			foreach (PetEquipInfo info in infos)
			{
				if (info.eqType == place)
				{
					return info;
				}
			}
			return null;
		}

		public bool RemoveEqPet(int petPlace, int eqPlace)
		{
			UsersPetInfo petAt = GetPetAt(petPlace);
			if (petAt == null)
			{
				return false;
			}
			PetEquipInfo eqPet = GetEqPet(petAt.PetEquips, eqPlace);
			if (eqPet == null)
			{
				return false;
			}
			ChangeEqPetToItem(eqPet);
			return petAt.PetEquips.Remove(eqPet);
		}

		public void ChangeEqPetToItem(PetEquipInfo info)
		{
			if (info.Template != null)
			{
				ItemInfo itemInfo = ItemInfo.CreateFromTemplate(info.Template, 1, 105);
				itemInfo.IsBinds = true;
				itemInfo.IsUsed = true;
				itemInfo.ValidDate = info.ValidDate;
				itemInfo.BeginDate = info.startTime;
				m_player.AddTemplate(itemInfo);
			}
		}

		public void RemoveAllEqPet(List<PetEquipInfo> infos)
		{
			foreach (PetEquipInfo info in infos)
			{
				ChangeEqPetToItem(info);
			}
		}

		public List<PetEquipInfo> DeserializePetEquip(string eqString)
		{
			if (string.IsNullOrEmpty(eqString))
			{
				return new List<PetEquipInfo>();
			}
			List<PetEquipInfo> list = JsonConvert.DeserializeObject<List<PetEquipInfo>>(eqString);
			List<PetEquipInfo> list2 = new List<PetEquipInfo>();
			foreach (PetEquipInfo item in list)
			{
				if (item.Template == null)
				{
					ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(item.eqTemplateID);
					if (itemTemplateInfo != null)
					{
						list2.Add(new PetEquipInfo(itemTemplateInfo)
						{
							eqTemplateID = item.eqTemplateID,
							eqType = item.eqType,
							ValidDate = item.ValidDate,
							startTime = item.startTime
						});
					}
				}
				else
				{
					list2.Add(item);
				}
			}
			return list2;
		}

		public string SerializePetEquip(List<PetEquipInfo> eqs)
		{
			return JsonConvert.SerializeObject(eqs);
		}

		public virtual bool OnChangedPetEquip(int place)
		{
			lock (m_lock)
			{
				if (m_pets[place] != null && m_pets[place].IsEquip)
				{
					m_player.EquipBag.UpdatePlayerProperties();
				}
			}
			OnPlaceChanged(place);
			return true;
		}

		public override bool RemovePet(UsersPetInfo pet)
		{
			if (!base.RemovePet(pet))
			{
				return false;
			}
			if (pet.PetEquips != null && pet.PetEquips.Count > 0)
			{
				RemoveAllEqPet(pet.PetEquips);
			}
			lock (list_0)
			{
				pet.IsExit = false;
				list_0.Add(pet);
			}
			return true;
		}

		public override void UpdateChangedPlaces()
		{
			m_player.Out.SendUpdateUserPet(this, m_changedPlaces.ToArray());
			base.UpdateChangedPlaces();
		}

		static PetInventory()
		{
			ilog_1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}