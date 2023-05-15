// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.AbstractInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
    public abstract class AbstractInventory
    {
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly ILog log;
        protected object m_lock;
        private int int_0;
        private int int_1;
        private int int_2;
        private bool bool_0;
        protected SqlDataProvider.Data.ItemInfo[] m_items;
        protected List<int> m_changedPlaces;
        private int int_3;

        public int BeginSlot => this.int_2;

        public int Capalility
        {
            get => this.int_1;
            set => this.int_1 = value < 0 ? 0 : (value > this.m_items.Length ? this.m_items.Length : value);
        }

        public int BagType => this.int_0;

        public bool IsEmpty(int slot) => slot < 0 || slot >= this.int_1 || this.m_items[slot] == null;

        public AbstractInventory(int capability, int type, int beginSlot, bool autoStack)
        {
            this.m_lock = new object();
            this.m_changedPlaces = new List<int>();
            this.int_1 = capability;
            this.int_0 = type;
            this.int_2 = beginSlot;
            this.bool_0 = autoStack;
            this.m_items = new SqlDataProvider.Data.ItemInfo[capability];
        }

        public virtual bool AddItem(SqlDataProvider.Data.ItemInfo item) => this.AddItem(item, this.int_2);

        public virtual bool AddItem(SqlDataProvider.Data.ItemInfo item, int minSlot)
        {
            if (item == null)
                return false;
            int firstEmptySlot = this.FindFirstEmptySlot(minSlot);
            return this.AddItemTo(item, firstEmptySlot);
        }

        public virtual bool AddItem(SqlDataProvider.Data.ItemInfo item, int minSlot, int maxSlot)
        {
            if (item == null)
                return false;
            int firstEmptySlot = this.FindFirstEmptySlot(minSlot, maxSlot);
            return this.AddItemTo(item, firstEmptySlot);
        }

        public virtual bool AddItemTo(SqlDataProvider.Data.ItemInfo item, int place)
        {
            if (item == null || place >= this.int_1 || place < 0)
                return false;
            lock (this.m_lock)
            {
                if (this.m_items[place] != null)
                {
                    place = -1;
                }
                else
                {
                    this.m_items[place] = item;
                    item.Place = place;
                    item.BagType = this.int_0;
                }
            }
            if (place != -1)
                this.OnPlaceChanged(place);
            return place != -1;
        }

        public virtual bool TakeOutItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == null)
                return false;
            int place = -1;
            lock (this.m_lock)
            {
                for (int index = 0; index < this.int_1; ++index)
                {
                    if (this.m_items[index] == item)
                    {
                        place = index;
                        this.m_items[index] = (SqlDataProvider.Data.ItemInfo)null;
                        break;
                    }
                }
            }
            if (place != -1)
            {
                this.OnPlaceChanged(place);
                if (item.BagType == this.BagType)
                {
                    item.Place = -1;
                    item.BagType = -1;
                }
            }
            return place != -1;
        }

        public bool TakeOutItemAt(int place) => this.TakeOutItem(this.GetItemAt(place));

        public void RemoveAllItem(List<SqlDataProvider.Data.ItemInfo> items)
        {
            this.BeginChanges();
            lock (this.m_lock)
            {
                foreach (SqlDataProvider.Data.ItemInfo itemInfo in items)
                {
                    if (itemInfo.Place >= this.m_items.Length)
                        AbstractInventory.ilog_0.Error((object)("ERROR PLACE OUT SIZE CAPALITITY: " + (object)itemInfo.Place + " - tempid: " + (object)itemInfo.TemplateID));
                    else if (this.m_items[itemInfo.Place] != null)
                        this.RemoveItem(this.m_items[itemInfo.Place]);
                }
            }
            this.CommitChanges();
        }

        public void RemoveAllItem(List<int> places)
        {
            this.BeginChanges();
            lock (this.m_lock)
            {
                for (int index = 0; index < places.Count; ++index)
                {
                    int place = places[index];
                    if (this.m_items[place] != null)
                        this.RemoveItem(this.m_items[place]);
                }
            }
            this.CommitChanges();
        }

        public virtual bool RemoveItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == null)
                return false;
            int place = -1;
            lock (this.m_lock)
            {
                for (int index = 0; index < this.int_1; ++index)
                {
                    if (this.m_items[index] == item)
                    {
                        place = index;
                        this.m_items[index] = (SqlDataProvider.Data.ItemInfo)null;
                        break;
                    }
                }
            }
            if (place != -1)
            {
                this.OnPlaceChanged(place);
                if (item.BagType == this.BagType)
                {
                    item.Place = -1;
                    item.BagType = -1;
                }
            }
            return place != -1;
        }

        public bool RemoveItemAt(int place) => this.RemoveItem(this.GetItemAt(place));

        public virtual bool AddCountToStack(SqlDataProvider.Data.ItemInfo item, int count)
        {
            if (item == null || count <= 0 || (item.BagType != this.int_0 || item.Count + count > item.Template.MaxCount))
                return false;
            item.Count += count;
            this.OnPlaceChanged(item.Place);
            return true;
        }

        public virtual bool RemoveCountFromStack(SqlDataProvider.Data.ItemInfo item, int count)
        {
            if (item == null || count <= 0 || (item.BagType != this.int_0 || item.Count < count))
                return false;
            if (item.Count == count)
                return this.RemoveItem(item);
            item.Count -= count;
            this.OnPlaceChanged(item.Place);
            return true;
        }

        public virtual bool AddTemplateAt(SqlDataProvider.Data.ItemInfo cloneItem, int count, int place) => this.AddTemplate(cloneItem, count, place, this.int_1 - 1);

        public virtual bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, int count) => this.AddTemplate(cloneItem, count, this.int_2, this.int_1 - 1);

        public virtual bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem) => this.AddTemplate(cloneItem, cloneItem.Count, this.int_2, this.int_1 - 1);

        public virtual bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, int count, int minSlot, int maxSlot)
        {
            if (cloneItem == null)
                return false;
            ItemTemplateInfo template = cloneItem.Template;
            if (template == null || count <= 0 || (minSlot < this.int_2 || minSlot > this.int_1 - 1) || (maxSlot < this.int_2 || maxSlot > this.int_1 - 1 || minSlot > maxSlot))
                return false;
            lock (this.m_lock)
            {
                List<int> intList = new List<int>();
                int num1 = count;
                for (int index = minSlot; index <= maxSlot; ++index)
                {
                    SqlDataProvider.Data.ItemInfo to = this.m_items[index];
                    if (to == null)
                    {
                        num1 -= template.MaxCount;
                        intList.Add(index);
                    }
                    else if (this.bool_0 && cloneItem.CanStackedTo(to))
                    {
                        num1 -= template.MaxCount - to.Count;
                        intList.Add(index);
                    }
                    if (num1 <= 0)
                        break;
                }
                if (num1 > 0)
                    return false;
                this.BeginChanges();
                try
                {
                    int num2 = count;
                    foreach (int place in intList)
                    {
                        SqlDataProvider.Data.ItemInfo itemInfo1 = this.m_items[place];
                        if (itemInfo1 == null)
                        {
                            SqlDataProvider.Data.ItemInfo itemInfo2 = cloneItem.Clone();
                            itemInfo2.Count = num2 < template.MaxCount ? num2 : template.MaxCount;
                            num2 -= itemInfo2.Count;
                            this.AddItemTo(itemInfo2, place);
                        }
                        else if (itemInfo1.TemplateID == template.TemplateID)
                        {
                            int num3 = itemInfo1.Count + num2 < template.MaxCount ? num2 : template.MaxCount - itemInfo1.Count;
                            itemInfo1.Count += num3;
                            num2 -= num3;
                            this.OnPlaceChanged(place);
                        }
                        else
                            AbstractInventory.ilog_0.Error((object)"Add template erro: select slot's TemplateId not equest templateId");
                    }
                    if (num2 != 0)
                        AbstractInventory.ilog_0.Error((object)"Add template error: last count not equal Zero.");
                }
                finally
                {
                    this.CommitChanges();
                }
                return true;
            }
        }

        public virtual bool RemoveTemplate(int templateId, int count) => this.RemoveTemplate(templateId, count, 0, this.int_1 - 1);

        public virtual bool RemoveTemplate(int templateId, int count, int minSlot, int maxSlot)
        {
            if (count <= 0 || minSlot < 0 || (minSlot > this.int_1 - 1 || maxSlot <= 0) || (maxSlot > this.int_1 - 1 || minSlot > maxSlot))
                return false;
            lock (this.m_lock)
            {
                List<int> intList = new List<int>();
                int num1 = count;
                for (int index = minSlot; index <= maxSlot; ++index)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = this.m_items[index];
                    if (itemInfo != null && itemInfo.TemplateID == templateId)
                    {
                        intList.Add(index);
                        num1 -= itemInfo.Count;
                        if (num1 <= 0)
                            break;
                    }
                }
                if (num1 > 0)
                    return false;
                this.BeginChanges();
                int num2 = count;
                try
                {
                    foreach (int place in intList)
                    {
                        SqlDataProvider.Data.ItemInfo itemInfo = this.m_items[place];
                        if (itemInfo != null && itemInfo.TemplateID == templateId)
                        {
                            if (itemInfo.Count <= num2)
                            {
                                this.RemoveItem(itemInfo);
                                num2 -= itemInfo.Count;
                            }
                            else
                            {
                                int num3 = itemInfo.Count - num2 < itemInfo.Count ? num2 : 0;
                                itemInfo.Count -= num3;
                                num2 -= num3;
                                this.OnPlaceChanged(place);
                            }
                        }
                    }
                    if (num2 != 0)
                        AbstractInventory.ilog_0.Error((object)"Remove templat error:last itemcoutj not equal Zero.");
                }
                finally
                {
                    this.CommitChanges();
                }
                return true;
            }
        }

        public virtual bool MoveItem(int fromSlot, int toSlot, int count)
        {
            if (fromSlot < 0 || toSlot < 0 || (fromSlot >= this.int_1 || toSlot >= this.int_1) || fromSlot == toSlot)
                return false;
            bool flag = false;
            lock (this.m_lock)
                flag = this.CombineItems(fromSlot, toSlot) || this.StackItems(fromSlot, toSlot, count) || this.ExchangeItems(fromSlot, toSlot);
            if (flag)
            {
                this.BeginChanges();
                try
                {
                    this.OnPlaceChanged(fromSlot);
                    this.OnPlaceChanged(toSlot);
                }
                finally
                {
                    this.CommitChanges();
                }
            }
            return flag;
        }

        public bool IsSolt(int slot) => slot >= 0 && slot < this.int_1;

        public void ClearBag()
        {
            this.BeginChanges();
            lock (this.m_lock)
            {
                for (int int2 = this.int_2; int2 < this.int_1; ++int2)
                {
                    if (this.m_items[int2] != null)
                        this.RemoveItem(this.m_items[int2]);
                }
            }
            this.CommitChanges();
        }

        public void ClearBagWithoutPlace(int place)
        {
            this.BeginChanges();
            lock (this.m_lock)
            {
                for (int int2 = this.int_2; int2 < this.int_1; ++int2)
                {
                    if (this.m_items[int2] != null && this.m_items[int2].Place != place)
                        this.RemoveItem(this.m_items[int2]);
                }
            }
            this.CommitChanges();
        }

        public bool StackItemToAnother(SqlDataProvider.Data.ItemInfo item)
        {
            lock (this.m_lock)
            {
                for (int index = this.int_1 - 1; index >= 0; --index)
                {
                    if (item != null && this.m_items[index] != null && (this.m_items[index] != item && item.CanStackedTo(this.m_items[index])) && this.m_items[index].Count + item.Count <= item.Template.MaxCount)
                    {
                        this.m_items[index].Count += item.Count;
                        item.IsExist = false;
                        item.RemoveType = 26;
                        this.UpdateItem(this.m_items[index]);
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool CombineItems(int fromSlot, int toSlot) => false;

        protected virtual bool StackItems(int fromSlot, int toSlot, int itemCount)
        {
            SqlDataProvider.Data.ItemInfo to = this.m_items[fromSlot];
            SqlDataProvider.Data.ItemInfo itemInfo1 = this.m_items[toSlot];
            if (itemCount == 0 || itemCount > to.Count)
                itemCount = to.Count <= 0 ? 1 : to.Count;
            if (itemInfo1 != null && itemInfo1.TemplateID == to.TemplateID && itemInfo1.CanStackedTo(to))
            {
                if (to.Count + itemInfo1.Count > to.Template.MaxCount)
                {
                    to.Count -= itemInfo1.Template.MaxCount - itemInfo1.Count;
                    itemInfo1.Count = itemInfo1.Template.MaxCount;
                }
                else
                {
                    if (itemCount >= to.Count)
                        this.RemoveItem(to);
                    else
                        to.Count -= itemCount;
                    itemInfo1.Count += itemCount;
                }
                return true;
            }
            if (itemInfo1 != null || to.Count <= itemCount)
                return false;
            SqlDataProvider.Data.ItemInfo itemInfo2 = to.Clone();
            itemInfo2.Count = itemCount;
            if (!this.AddItemTo(itemInfo2, toSlot))
                return false;
            to.Count -= itemCount;
            return true;
        }

        protected virtual bool ExchangeItems(int fromSlot, int toSlot)
        {
            SqlDataProvider.Data.ItemInfo itemInfo1 = this.m_items[toSlot];
            SqlDataProvider.Data.ItemInfo itemInfo2 = this.m_items[fromSlot];
            this.m_items[fromSlot] = itemInfo1;
            this.m_items[toSlot] = itemInfo2;
            if (itemInfo1 != null)
                itemInfo1.Place = fromSlot;
            if (itemInfo2 != null)
                itemInfo2.Place = toSlot;
            return true;
        }

        public virtual SqlDataProvider.Data.ItemInfo GetItemAt(int slot) => slot >= 0 && slot < this.int_1 ? this.m_items[slot] : (SqlDataProvider.Data.ItemInfo)null;

        public int FindFirstEmptySlot() => this.FindFirstEmptySlot(this.int_2);

        public virtual int FindFirstEmptySlot(int minSlot)
        {
            if (minSlot >= this.int_1)
                return -1;
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] == null)
                        return index;
                }
                return -1;
            }
        }

        public int CountTotalEmptySlot() => this.CountTotalEmptySlot(this.int_2);

        public int CountTotalEmptySlot(int minSlot)
        {
            if (minSlot >= this.int_1)
                return -1;
            lock (this.m_lock)
            {
                int num = 0;
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] == null)
                        ++num;
                }
                return num;
            }
        }

        public int FindFirstEmptySlot(int minSlot, int maxSlot)
        {
            if (minSlot >= maxSlot)
                return -1;
            lock (this.m_lock)
            {
                for (int index = minSlot; index < maxSlot; ++index)
                {
                    if (this.m_items[index] == null)
                        return index;
                }
                return -1;
            }
        }

        public int FindLastEmptySlot()
        {
            lock (this.m_lock)
            {
                for (int index = this.int_1 - 1; index >= 0; --index)
                {
                    if (this.m_items[index] == null)
                        return index;
                }
                return -1;
            }
        }

        public int FindLastEmptySlot(int maxSlot)
        {
            lock (this.m_lock)
            {
                for (int index = maxSlot - 1; index >= 0; --index)
                {
                    if (this.m_items[index] == null)
                        return index;
                }
                return -1;
            }
        }

        public virtual void Clear()
        {
            this.BeginChanges();
            lock (this.m_lock)
            {
                for (int place = 0; place < this.int_1; ++place)
                {
                    this.m_items[place] = (SqlDataProvider.Data.ItemInfo)null;
                    this.OnPlaceChanged(place);
                }
            }
            this.CommitChanges();
        }

        public virtual SqlDataProvider.Data.ItemInfo GetItemByCategoryID(
          int minSlot,
          int categoryID,
          int property)
        {
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] != null && this.m_items[index].Template.CategoryID == categoryID && (property == -1 || this.m_items[index].Template.Property1 == property))
                        return this.m_items[index];
                }
                return (SqlDataProvider.Data.ItemInfo)null;
            }
        }

        public virtual SqlDataProvider.Data.ItemInfo GetItemByTemplateID(
          int minSlot,
          int templateId)
        {
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] != null && this.m_items[index].TemplateID == templateId)
                        return this.m_items[index];
                }
                return (SqlDataProvider.Data.ItemInfo)null;
            }
        }

        public virtual List<SqlDataProvider.Data.ItemInfo> GetItemsByTemplateID(
          int minSlot,
          int templateid)
        {
            lock (this.m_lock)
            {
                List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] != null && this.m_items[index].TemplateID == templateid)
                        itemInfoList.Add(this.m_items[index]);
                }
                return itemInfoList;
            }
        }

        public virtual SqlDataProvider.Data.ItemInfo GetItemByItemID(int minSlot, int itemId)
        {
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] != null && this.m_items[index].ItemID == itemId)
                        return this.m_items[index];
                }
                return (SqlDataProvider.Data.ItemInfo)null;
            }
        }

        public virtual int GetItemCount(int templateId) => this.GetItemCount(this.int_2, templateId);

        public int GetItemCount(int minSlot, int templateId)
        {
            int num = 0;
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] != null && this.m_items[index].TemplateID == templateId)
                        num += this.m_items[index].Count;
                }
            }
            return num;
        }

        public virtual List<SqlDataProvider.Data.ItemInfo> GetItems() => this.GetItems(0, this.int_1);

        public virtual List<SqlDataProvider.Data.ItemInfo> GetItems(
          int minSlot,
          int maxSlot)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            lock (this.m_lock)
            {
                for (int index = minSlot; index < maxSlot; ++index)
                {
                    if (this.m_items[index] != null)
                        itemInfoList.Add(this.m_items[index]);
                }
            }
            return itemInfoList;
        }

        public int GetEmptyCount() => this.GetEmptyCount(this.int_2);

        public virtual int GetEmptyCount(int minSlot)
        {
            if (minSlot < 0 || minSlot > this.int_1 - 1)
                return 0;
            int num = 0;
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.int_1; ++index)
                {
                    if (this.m_items[index] == null)
                        ++num;
                }
            }
            return num;
        }

        public virtual void UseItem(SqlDataProvider.Data.ItemInfo item)
        {
            bool flag = false;
            if (!item.IsBinds && (item.Template.BindType == 2 || item.Template.BindType == 3))
            {
                item.IsBinds = true;
                flag = true;
            }
            if (!item.IsUsed)
            {
                item.IsUsed = true;
                item.BeginDate = DateTime.Now;
                flag = true;
            }
            if (!flag)
                return;
            this.OnPlaceChanged(item.Place);
        }

        public virtual void UpdateItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == null || item.BagType != this.int_0)
                return;
            if (item.Count <= 0)
                this.RemoveItem(item);
            else
                this.OnPlaceChanged(item.Place);
        }

        public virtual bool RemoveCountFromStack(SqlDataProvider.Data.ItemInfo item, int count, eItemRemoveType type)
        {
            if (item == null || count <= 0 || (item.BagType != this.int_0 || item.Count < count))
                return false;
            if (item.Count == count)
                return this.RemoveItem(item);
            item.Count -= count;
            this.OnPlaceChanged(item.Place);
            return true;
        }

        public virtual bool RemoveItem(SqlDataProvider.Data.ItemInfo item, eItemRemoveType type)
        {
            if (item == null)
                return false;
            int place = -1;
            lock (this.m_lock)
            {
                for (int index = 0; index < this.int_1; ++index)
                {
                    if (this.m_items[index] == item)
                    {
                        place = index;
                        this.m_items[index] = (SqlDataProvider.Data.ItemInfo)null;
                        break;
                    }
                }
            }
            if (place != -1)
            {
                this.OnPlaceChanged(place);
                if (item.BagType == this.BagType && item.Place == place)
                {
                    item.Place = -1;
                    item.BagType = -1;
                }
            }
            return place != -1;
        }

        protected void OnPlaceChanged(int place)
        {
            if (!this.m_changedPlaces.Contains(place))
                this.m_changedPlaces.Add(place);
            if (this.int_3 > 0 || this.m_changedPlaces.Count <= 0)
                return;
            this.UpdateChangedPlaces();
        }

        public void BeginChanges() => Interlocked.Increment(ref this.int_3);

        public void CommitChanges()
        {
            int num = Interlocked.Decrement(ref this.int_3);
            if (num < 0)
            {
                if (AbstractInventory.ilog_0.IsErrorEnabled)
                    AbstractInventory.ilog_0.Error((object)("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace));
                Thread.VolatileWrite(ref this.int_3, 0);
            }
            if (num > 0 || this.m_changedPlaces.Count <= 0)
                return;
            this.UpdateChangedPlaces();
        }

        public virtual void UpdateChangedPlaces() => this.m_changedPlaces.Clear();

        public SqlDataProvider.Data.ItemInfo[] GetRawSpaces()
        {
            lock (this.m_lock)
                return this.m_items.Clone() as SqlDataProvider.Data.ItemInfo[];
        }
    }
}
