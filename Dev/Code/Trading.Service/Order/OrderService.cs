






using System;
using System.Data.Entity;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.Order
{
	public class OrderService : IOrderService
	{
		private readonly Zerg.Common.Data.ITradingRepository<OrderEntity> _orderRepository;
		private readonly ILog _log;

		public OrderService(Zerg.Common.Data.ITradingRepository<OrderEntity> orderRepository,ILog log)
		{
			_orderRepository = orderRepository;
			_log = log;
		}
		
		public OrderEntity Create (OrderEntity entity)
		{
			try
            {
                _orderRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(OrderEntity entity)
		{
			try
            {
                _orderRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public OrderEntity Update (OrderEntity entity)
		{
			try
            {
                _orderRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public OrderEntity GetOrderById (int id)
		{
			try
            {
                return _orderRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

        /// <summary>
        /// 根据经济人Id查找订单；
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IQueryable<OrderEntity> GetOrdersByAgent(int agentId)
        {
            var query = _orderRepository.Table;
            try
            {
                query = query.Where(q=>q.AgentId==agentId);
                return query.OrderBy(q=>q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        /// <summary>
        /// 根据地产商Id查找Order；
        /// </summary>
        /// <param name="busId"></param>
        /// <returns></returns>
        public IQueryable<OrderEntity> GetOrdersByBus(int busId)
        {
            var query = _orderRepository.Table;
            try
            {
                query = query.Where(q => q.BusId == busId);
                return query.OrderBy(q => q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
		public IQueryable<OrderEntity> GetOrdersByCondition(OrderSearchCondition condition)
		{
			var query = _orderRepository.Table;
			try
			{

				if (condition.AdddateBegin.HasValue)
                {
                    query = query.Where(q => q.Adddate>= condition.AdddateBegin.Value);
                }
                if (condition.AdddateEnd.HasValue)
                {
                    query = query.Where(q => q.Adddate < condition.AdddateEnd.Value);
                }


				if (condition.UpddateBegin.HasValue)
                {
                    query = query.Where(q => q.Upddate>= condition.UpddateBegin.Value);
                }
                if (condition.UpddateEnd.HasValue)
                {
                    query = query.Where(q => q.Upddate < condition.UpddateEnd.Value);
                }


				if (condition.Ordertype.HasValue)
                {
                    query = query.Where(q => q.Ordertype == condition.Ordertype.Value);
                }


			    if (condition.Shipstatus.HasValue)
			    {
			        query = query.Where(q => q.Shipstatus == condition.Shipstatus.Value);
			    }


                if (condition.Shipstatuses != null && condition.Shipstatuses.Any())
                {
                    query = query.Where(q => condition.Shipstatuses.Contains(q.Shipstatus));
                }



				if (!string.IsNullOrEmpty(condition.Ordercode))
                {
                    query = query.Where(q => q.Ordercode.Contains(condition.Ordercode));
                }



				if (!string.IsNullOrEmpty(condition.Busname))
                {
                    query = query.Where(q => q.Busname.Contains(condition.Busname));
                }



				if (!string.IsNullOrEmpty(condition.Agentname))
                {
                    query = query.Where(q => q.Agentname.Contains(condition.Agentname));
                }



				if (!string.IsNullOrEmpty(condition.Agenttel))
                {
                    query = query.Where(q => q.Agenttel.Contains(condition.Agenttel));
                }



				if (!string.IsNullOrEmpty(condition.Customname))
                {
                    query = query.Where(q => q.Customname.Contains(condition.Customname));
                }



				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }



				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.OrderDetails != null && condition.OrderDetails.Any())
                {
                    query = query.Where(q => condition.OrderDetails.Contains(q.OrderDetail));
                }



                if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
                if (condition.BusId.HasValue)
                {
                    query = query.Where(q => q.BusId == condition.BusId.Value);
                }
                if (condition.AgentId.HasValue)
                {
                    query = query.Where(q => q.AgentId == condition.AgentId.Value);
                }

             




				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumOrderSearchOrderBy.OrderById:
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

		public int GetOrderCount (OrderSearchCondition condition)
		{
			var query = _orderRepository.Table;
			try
			{

				if (condition.AdddateBegin.HasValue)
                {
                    query = query.Where(q => q.Adddate>= condition.AdddateBegin.Value);
                }
                if (condition.AdddateEnd.HasValue)
                {
                    query = query.Where(q => q.Adddate < condition.AdddateEnd.Value);
                }


				if (condition.UpddateBegin.HasValue)
                {
                    query = query.Where(q => q.Upddate>= condition.UpddateBegin.Value);
                }
                if (condition.UpddateEnd.HasValue)
                {
                    query = query.Where(q => q.Upddate < condition.UpddateEnd.Value);
                }


				if (condition.Ordertype.HasValue)
                {
                    query = query.Where(q => q.Ordertype == condition.Ordertype.Value);
                }

                if (condition.Shipstatus.HasValue)
                {
                    query = query.Where(q => q.Shipstatus == condition.Shipstatus.Value);
                }


                if (condition.Shipstatuses != null && condition.Shipstatuses.Any())
                {
                    query = query.Where(q => condition.Shipstatuses.Contains(q.Shipstatus));
                }





				if (!string.IsNullOrEmpty(condition.Ordercode))
                {
                    query = query.Where(q => q.Ordercode.Contains(condition.Ordercode));
                }



				if (!string.IsNullOrEmpty(condition.Busname))
                {
                    query = query.Where(q => q.Busname.Contains(condition.Busname));
                }



				if (!string.IsNullOrEmpty(condition.Agentname))
                {
                    query = query.Where(q => q.Agentname.Contains(condition.Agentname));
                }



				if (!string.IsNullOrEmpty(condition.Agenttel))
                {
                    query = query.Where(q => q.Agenttel.Contains(condition.Agenttel));
                }



				if (!string.IsNullOrEmpty(condition.Customname))
                {
                    query = query.Where(q => q.Customname.Contains(condition.Customname));
                }



				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }



				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.OrderDetails != null && condition.OrderDetails.Any())
                {
                    query = query.Where(q => condition.OrderDetails.Contains(q.OrderDetail));
                }



                if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
                if (condition.BusId.HasValue)
                {
                    query = query.Where(q => q.BusId == condition.BusId.Value);
                }
                if (condition.AgentId.HasValue)
                {
                    query = query.Where(q => q.AgentId == condition.AgentId.Value);
                }



				return query.Count();
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}

        /// <summary>
        /// 生成订单号
        /// 订单号由时间+传入的type标识符+流水号组成25位定长string
        /// </summary>
        /// <returns>type为右起10，11位类型标识符，1为推荐，2为带客，3为成交</returns>
        public string CreateOrderNumber(int type)
        {
            //获取当日流水号
            var num = GetOrderCount(new OrderSearchCondition
            {
                AdddateBegin = DateTime.Today,
                AdddateEnd = DateTime.Today.AddDays(1)
            });

            return DateTime.Now.ToString("yyyyMMddHHmmss")+(type).ToString("00")+(num+1).ToString("000000000");
        }
    }
}