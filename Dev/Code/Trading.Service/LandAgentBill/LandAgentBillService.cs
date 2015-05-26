






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.LandAgentBill
{
	public class LandAgentBillService : ILandAgentBillService
	{
		private readonly Zerg.Common.Data.ITradingRepository<LandAgentBillEntity> _landagentbillRepository;
		private readonly ILog _log;

		public LandAgentBillService(Zerg.Common.Data.ITradingRepository<LandAgentBillEntity> landagentbillRepository,ILog log)
		{
			_landagentbillRepository = landagentbillRepository;
			_log = log;
		}
		
		public LandAgentBillEntity Create (LandAgentBillEntity entity)
		{
			try
            {
                _landagentbillRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(LandAgentBillEntity entity)
		{
			try
            {
                _landagentbillRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public LandAgentBillEntity Update (LandAgentBillEntity entity)
		{
			try
            {
                _landagentbillRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public LandAgentBillEntity GetLandAgentBillById (int id)
		{
			try
            {
                return _landagentbillRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<LandAgentBillEntity> GetLandAgentBillsByCondition(LandAgentBillSearchCondition condition)
		{
			var query = _landagentbillRepository.Table;
			try
			{

				if (condition.AmountBegin.HasValue)
                {
                    query = query.Where(q => q.Amount>= condition.AmountBegin.Value);
                }
                if (condition.AmountEnd.HasValue)
                {
                    query = query.Where(q => q.Amount < condition.AmountEnd.Value);
                }


				if (condition.ActualamountBegin.HasValue)
                {
                    query = query.Where(q => q.Actualamount>= condition.ActualamountBegin.Value);
                }
                if (condition.ActualamountEnd.HasValue)
                {
                    query = query.Where(q => q.Actualamount < condition.ActualamountEnd.Value);
                }


				if (condition.CheckoutdateBegin.HasValue)
                {
                    query = query.Where(q => q.Checkoutdate>= condition.CheckoutdateBegin.Value);
                }
                if (condition.CheckoutdateEnd.HasValue)
                {
                    query = query.Where(q => q.Checkoutdate < condition.CheckoutdateEnd.Value);
                }


				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }


				if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime>= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


				if (condition.Isinvoice.HasValue)
                {
                    query = query.Where(q => q.Isinvoice == condition.Isinvoice.Value);
                }



				if (condition.AgentId.HasValue)
                {
                    query = query.Where(q =>q.AgentId==condition.AgentId.Value);
                }



				if (!string.IsNullOrEmpty(condition.Agentname))
                {
                    query = query.Where(q => q.Agentname.Contains(condition.Agentname));
                }



				if (!string.IsNullOrEmpty(condition.Landagentname))
                {
                    query = query.Where(q => q.Landagentname.Contains(condition.Landagentname));
                }



				if (!string.IsNullOrEmpty(condition.Cardnumber))
                {
                    query = query.Where(q => q.Cardnumber.Contains(condition.Cardnumber));
                }



				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }



				if (!string.IsNullOrEmpty(condition.Beneficiary))
                {
                    query = query.Where(q => q.Beneficiary.Contains(condition.Beneficiary));
                }



				if (!string.IsNullOrEmpty(condition.Beneficiarynumber))
                {
                    query = query.Where(q => q.Beneficiarynumber.Contains(condition.Beneficiarynumber));
                }



				if (!string.IsNullOrEmpty(condition.Customname))
                {
                    query = query.Where(q => q.Customname.Contains(condition.Customname));
                }



				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }


				if (condition.Orders != null && condition.Orders.Any())
                {
                    query = query.Where(q => condition.Orders.Contains(q.Order));
                }


                if (condition.LandagentId.HasValue)
                {
                    query = query.Where(q => q.LandagentId == condition.LandagentId);
                }




				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumLandAgentBillSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;

                    }
					
				}

				else
				{
					query = query.OrderBy(q=>q.Id);
				}

				if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1)*condition.PageCount.Value).Take(condition.PageCount.Value);
                }
				return query;
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return null;
			}
		}

		public int GetLandAgentBillCount (LandAgentBillSearchCondition condition)
		{
			var query = _landagentbillRepository.Table;
			try
			{

				if (condition.AmountBegin.HasValue)
                {
                    query = query.Where(q => q.Amount>= condition.AmountBegin.Value);
                }
                if (condition.AmountEnd.HasValue)
                {
                    query = query.Where(q => q.Amount < condition.AmountEnd.Value);
                }


				if (condition.ActualamountBegin.HasValue)
                {
                    query = query.Where(q => q.Actualamount>= condition.ActualamountBegin.Value);
                }
                if (condition.ActualamountEnd.HasValue)
                {
                    query = query.Where(q => q.Actualamount < condition.ActualamountEnd.Value);
                }


				if (condition.CheckoutdateBegin.HasValue)
                {
                    query = query.Where(q => q.Checkoutdate>= condition.CheckoutdateBegin.Value);
                }
                if (condition.CheckoutdateEnd.HasValue)
                {
                    query = query.Where(q => q.Checkoutdate < condition.CheckoutdateEnd.Value);
                }


				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }


				if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime>= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


				if (condition.Isinvoice.HasValue)
                {
                    query = query.Where(q => q.Isinvoice == condition.Isinvoice.Value);
                }



                if (condition.AgentId.HasValue)
                {
                    query = query.Where(q => q.AgentId == condition.AgentId.Value);
                }



				if (!string.IsNullOrEmpty(condition.Agentname))
                {
                    query = query.Where(q => q.Agentname.Contains(condition.Agentname));
                }



				if (!string.IsNullOrEmpty(condition.Landagentname))
                {
                    query = query.Where(q => q.Landagentname.Contains(condition.Landagentname));
                }



				if (!string.IsNullOrEmpty(condition.Cardnumber))
                {
                    query = query.Where(q => q.Cardnumber.Contains(condition.Cardnumber));
                }



				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }



				if (!string.IsNullOrEmpty(condition.Beneficiary))
                {
                    query = query.Where(q => q.Beneficiary.Contains(condition.Beneficiary));
                }



				if (!string.IsNullOrEmpty(condition.Beneficiarynumber))
                {
                    query = query.Where(q => q.Beneficiarynumber.Contains(condition.Beneficiarynumber));
                }



				if (!string.IsNullOrEmpty(condition.Customname))
                {
                    query = query.Where(q => q.Customname.Contains(condition.Customname));
                }



				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }


				if (condition.Orders != null && condition.Orders.Any())
                {
                    query = query.Where(q => condition.Orders.Contains(q.Order));
                }


                if (condition.LandagentId.HasValue)
                {
                    query = query.Where(q => q.LandagentId == condition.LandagentId);
                }



				return query.Count();
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}
	}
}