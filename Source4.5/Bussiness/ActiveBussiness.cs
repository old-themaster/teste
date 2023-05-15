using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
    public class ActiveBussiness : BaseCrossBussiness
    {
        public ActiveInfo[] GetAllActives()
        {
            List<ActiveInfo> activeInfoList = new List<ActiveInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Active_All");
                while (ResultDataReader.Read())
                    activeInfoList.Add(this.InitActiveInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return activeInfoList.ToArray();
        }

        public ActiveConvertItemInfo[] GetSingleActiveConvertItems(int activeID)
        {
            List<ActiveConvertItemInfo> activeConvertItemInfoList = new List<ActiveConvertItemInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)activeID;
                this.db.GetReader(ref ResultDataReader, "SP_Active_Convert_Item_Info_Single", SqlParameters);
                while (ResultDataReader.Read())
                    activeConvertItemInfoList.Add(this.InitActiveConvertItemInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return activeConvertItemInfoList.ToArray();
        }

        public ActiveInfo GetSingleActives(int activeID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)activeID;
                this.db.GetReader(ref ResultDataReader, "SP_Active_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitActiveInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return (ActiveInfo)null;
        }

        public ActiveConvertItemInfo InitActiveConvertItemInfo(SqlDataReader reader)
        {
            return new ActiveConvertItemInfo()
            {
                ID = (int)reader["ID"],
                ActiveID = (int)reader["ActiveID"],
                TemplateID = (int)reader["TemplateID"],
                ItemType = (int)reader["ItemType"],
                ItemCount = (int)reader["ItemCount"],
                LimitValue = (int)reader["LimitValue"],
                IsBind = (bool)reader["IsBind"],
                ValidDate = (int)reader["ValidDate"]
            };
        }

        public ActiveInfo InitActiveInfo(SqlDataReader reader)
        {
            ActiveInfo activeInfo = new ActiveInfo()
            {
                ActiveID = (int)reader["ActiveID"],
                Description = reader["Description"] == null ? "" : reader["Description"].ToString(),
                Content = reader["Content"] == null ? "" : reader["Content"].ToString(),
                AwardContent = reader["AwardContent"] == null ? "" : reader["AwardContent"].ToString(),
                HasKey = (int)reader["HasKey"]
            };
            if (!string.IsNullOrEmpty(reader["EndDate"].ToString()))
                activeInfo.EndDate = new DateTime?((DateTime)reader["EndDate"]);
            activeInfo.IsOnly = (int)reader["IsOnly"];
            activeInfo.StartDate = (DateTime)reader["StartDate"];
            activeInfo.Title = reader["Title"].ToString();
            activeInfo.Type = (int)reader["Type"];
            activeInfo.ActiveType = (int)reader["ActiveType"];
            activeInfo.ActionTimeContent = reader["ActionTimeContent"] == null ? "" : reader["ActionTimeContent"].ToString();
            activeInfo.IsAdvance = (bool)reader["IsAdvance"];
            activeInfo.GoodsExchangeTypes = reader["GoodsExchangeTypes"] == null ? "" : reader["GoodsExchangeTypes"].ToString();
            activeInfo.GoodsExchangeNum = reader["GoodsExchangeNum"] == null ? "" : reader["GoodsExchangeNum"].ToString();
            activeInfo.limitType = reader["limitType"] == null ? "" : reader["limitType"].ToString();
            activeInfo.limitValue = reader["limitValue"] == null ? "" : reader["limitValue"].ToString();
            activeInfo.IsShow = (bool)reader["IsShow"];
            activeInfo.IconID = (int)reader["IconID"];
            return activeInfo;
        }
        //evento novo
        public List<ActivityQuestInfo> GetAllActivitysQuestInfos()
        {
            var tmp = new List<ActivityQuestInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Get_AllActivity_Quest");
                while (resultDataReader.Read())
                    tmp.Add(AutoInit<ActivityQuestInfo>(resultDataReader));
            }
            catch (Exception exception)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error("Init", exception);
            }
            finally
            {
                if ((resultDataReader != null) && !resultDataReader.IsClosed)
                    resultDataReader.Close();
            }
            return tmp;
        }

        public List<AtivityQuestConditionInfo> GetAllActivitysQuestConditions()
        {
            var tmp = new List<AtivityQuestConditionInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Get_AllActivity_Quest_Condiction");
                while (resultDataReader.Read())
                    tmp.Add(AutoInit<AtivityQuestConditionInfo>(resultDataReader));
            }
            catch (Exception exception)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error("Init", exception);
            }
            finally
            {
                if ((resultDataReader != null) && !resultDataReader.IsClosed)
                    resultDataReader.Close();
            }
            return tmp;
        }

        public List<AtivityQuestGoodsInfo> GetAllActivitysQuestGoods()
        {
            var tmp = new List<AtivityQuestGoodsInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Get_AllActivity_Quest_Goods");
                while (resultDataReader.Read())
                    tmp.Add(AutoInit<AtivityQuestGoodsInfo>(resultDataReader));
            }
            catch (Exception exception)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error("Init", exception);
            }
            finally
            {
                if ((resultDataReader != null) && !resultDataReader.IsClosed)
                    resultDataReader.Close();
            }
            return tmp;
        }

        internal T AutoInit<T>(SqlDataReader reader)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            for (int x = 0; x < reader.FieldCount; x++)
            {
                if (obj.GetType().GetProperty(reader.GetName(x)) != null)
                {
                    obj.GetType().GetProperty(reader.GetName(x)).SetValue(obj, reader.GetValue(x));
                }

            }
            return obj;
        }
        //
        public int PullDown(int activeID, string awardID, int userID, ref string msg)
        {
            int result = 1;
            try
            {
                SqlParameter[] array = new SqlParameter[]
                {
                    new SqlParameter("@ActiveID", activeID),
                    new SqlParameter("@AwardID", awardID),
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[3].Direction = ParameterDirection.ReturnValue;
                if (this.db2.RunProcedure("SP_Active_PullDown", array))
                {
                    result = (int)array[3].Value;
                    switch (result)
                    {
                        case 0:
                            msg = "A recompensa foi enviada para seu correio!";
                            break;
                        case 1:
                            msg = "Erro desconhecido.";
                            break;
                        case 2:
                            msg = "Nome dungkhong existe.";
                            break;
                        case 3:
                            msg = "Obter itens que falharam.";
                            break;
                        case 4:
                            msg = "Este número não existe, por favor, volte.";
                            break;
                        case 5:
                            msg = "prêmio Esta questão tem recebido não consegue mais.";
                            break;
                        case 6:
                            msg = "Você recebeu este prêmio antes.";
                            break;
                        case 7:
                            msg = "Atividade não começou.";
                            break;
                        case 8:
                            msg = "Atividade em atraso.";
                            break;
                        default:
                            msg = "Obter falha recompensado.";
                            break;
                    }
                }
            }
            catch (System.Exception exception)
            {
                if (this.log.IsErrorEnabled)
                {
                    this.log.Error("Init", exception);
                }
            }
            return result;
        }
    }
}
