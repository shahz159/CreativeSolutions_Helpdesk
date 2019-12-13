using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class TicketService: ITicketService
    {
        private readonly ITicketModel model;
        public TicketService(TicketModel _model)
        {
            model = _model;
        }
        public TicketDTO InsertTicketRequest(TicketDTO obj)
        {
            try
            {
                var data = model.NewTicketRequest(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                        
                    }
                }
                else
                    obj.message = "0";
                if (obj.message == "1")
                {
                    //Send Email
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public TicketDTO GetUnderApprovalTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetApprovalTickets(obj);
            return obj;
        }

        public TicketDTO UpdateTicketStatus(TicketDTO obj)
        {
            try
            {
                var data = model.UpdateTicketStatus(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }

        public TicketDTO AddResponseTime(TicketDTO obj)
        {
            try
            {
                var data = model.AddResponseTieme(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public TicketDTO AddSparePartRequest(TicketDTO obj)
        {
            try
            {
                var data = model.AddSparePartRequest(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public TicketDTO Addcomments(TicketDTO obj)
        {
            try
            {
                var data = model.Addcomments(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        
        public TicketDTO GetSystemUserProducts(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserProducts(obj);
            return obj;
        }
        public TicketDTO GetSystemUserModels(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserModels(obj);
            return obj;
        }
        public TicketDTO GetAccounts(TicketDTO obj)
        {
            obj.datasetxml = model.GetAccounts(obj);
            return obj;
        }
        public TicketDTO GetProducts(TicketDTO obj)
        {
            obj.datasetxml = model.Getproducts(obj);
            return obj;
        }
        
        public TicketDTO GetSystemUserTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetSystemUserTickets(obj);
            return obj;
        }
        public TicketDTO GetServiceEngineerTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetServiceEngineerTickets(obj);
            return obj;
        }
        public TicketDTO GetDashboardCount(TicketDTO obj)
        {
            obj.datasetxml = model.GetDashBoardCount(obj);
            return obj;
        }

        
        public TicketDTO GetSparePartRequestTickets(TicketDTO obj)
        {
            obj.datasetxml = model.GetSparePartRequestTickets(obj);
            return obj;
        }
        
        public TicketDTO GetServiceEngineerTicketsFiletrs(TicketDTO obj)
        {
            obj.datasetxml = model.GetServiceEngineerTicketsFilerts(obj);
            return obj;
        }
        public TicketDTO GetTicketDetails(TicketDTO obj)
        {
            obj.datasetxml = model.GetTicketDetails(obj);
            return obj;
        }
        public TicketDTO GetSparePartListById(TicketDTO obj)
        {
            obj.datasetxml = model.GetSparePartListByWarehouseId(obj);
            return obj;
        }
        
    }

    public interface ITicketService
    {
        TicketDTO InsertTicketRequest(TicketDTO obj);
        TicketDTO GetUnderApprovalTickets(TicketDTO obj);
        TicketDTO UpdateTicketStatus(TicketDTO obj);
        TicketDTO AddResponseTime(TicketDTO obj);
        TicketDTO AddSparePartRequest(TicketDTO obj);
        TicketDTO Addcomments(TicketDTO obj);

        TicketDTO GetSystemUserProducts(TicketDTO obj);
        TicketDTO GetSystemUserModels(TicketDTO obj);
        TicketDTO GetAccounts(TicketDTO obj);
        TicketDTO GetProducts(TicketDTO obj);
        TicketDTO GetSystemUserTickets(TicketDTO obj);
        TicketDTO GetServiceEngineerTickets(TicketDTO obj);
        TicketDTO GetDashboardCount(TicketDTO obj);
        TicketDTO GetSparePartRequestTickets(TicketDTO obj);
        TicketDTO GetServiceEngineerTicketsFiletrs(TicketDTO obj);

        TicketDTO GetTicketDetails(TicketDTO obj);
        TicketDTO GetSparePartListById(TicketDTO obj);
    }
}