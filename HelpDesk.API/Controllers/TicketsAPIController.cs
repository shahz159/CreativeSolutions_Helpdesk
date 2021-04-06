using HelpDesk.API.Bussiness;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HelpDesk.API.Controllers
{
    public class TicketsAPIController : ApiController
    {
        private readonly ITicketService service;
        public TicketsAPIController(TicketService _service)
        {
            service = _service;
        }
        /// <summary>
        /// New Ticket Request
        /// </summary>
        /// <param name="obj"></param>NewSystemUserTickets
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewInsertTicketRequest(TicketDTO obj)
        {
            var result = service.InsertTicketRequest(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get Under Approval Tickets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetUnderApprovalTickets(TicketDTO obj)
        {
            var result = service.GetUnderApprovalTickets(obj);
            return Ok(result);
        }
        /// <summary>
        /// Update Ticket Status
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewUpdateTicketStatus(TicketDTO obj)
        {
            var result = service.UpdateTicketStatus(obj);
            return Ok(result);
        }
        /// <summary>
        /// Ticket transfer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewTransferTicket(TicketDTO obj)
        {
            var result = service.TicketTransfer(obj);
            return Ok(result);
        }

        /// <summary>
        /// Add response time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewResponseTime(TicketDTO obj)
        {
            var result = service.AddResponseTime(obj);
            return Ok(result);
        }

        /// <summary>
        /// Get System User Products
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserProducts(TicketDTO obj)
        {
            var result = service.GetSystemUserProducts(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get System User Models
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserModels(TicketDTO obj)
        {
            var result = service.GetSystemUserModels(obj);
            return Ok(result);
        }
        /// <summary>
        /// get  accounts by company id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewAccountsByCompany(TicketDTO obj)
        {
            var result = service.GetAccounts(obj);
            return Ok(result);
        }
        /// <summary>
        /// get products by company id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewproductsByCompany(TicketDTO obj)
        {
            var result = service.GetProducts(obj);
            return Ok(result);
        }

        /// <summary>
        /// Get System User Ticktes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemUserTickets(TicketDTO obj)
        {
            var result = service.GetSystemUserTickets(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewRejectedTickets(TicketDTO obj)
        {
            var result = service.GetRejectedTickets(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get Service Engineer(user) tickets by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerTickets(TicketDTO obj)
        {
            var result = service.GetServiceEngineerTickets(obj);
            return Ok(result);
        }
        /// <summary>
        /// get dashboard count by user id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewDashBoardCount(TicketDTO obj)
        {
            var result = service.GetDashboardCount(obj);
            return Ok(result);
        }
        /// <summary>
        /// Get System Manager UserId 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSystemManagerId(TicketDTO obj)
        {
            var result = service.GetSystemManagerId(obj);
            return Ok(result);
        }

        /// <summary>
        /// get filters of service engineer tickets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewServiceEngineerTicketsFileters(TicketDTO obj)
        {
            var result = service.GetServiceEngineerTicketsFiletrs(obj);
            return Ok(result);
        }
        /// <summary>
        /// get ticket details by ticket number
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetTicketDetailsById(TicketDTO obj)
        {
            var result = service.GetTicketDetails(obj);
            return Ok(result);
        }

        /// <summary>
        /// Get Spare part list by warehouseid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewGetSparePartsList(TicketDTO obj)
        {
            var result = service.GetSparePartListById(obj);
            return Ok(result);
        }
        /// <summary>
        /// Insert spare part request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSparePartRequest(TicketDTO obj)
        {
            var result = service.AddSparePartRequest(obj);
            return Ok(result);
        }
        /// <summary>
        /// Add comments
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewComments(TicketDTO obj)
        {
            var result = service.Addcomments(obj);
            return Ok(result);
        }
        /// <summary>
        /// get Spare part request tickets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewSparePartRequestTickets(TicketDTO obj)
        {
            var result = service.GetSparePartRequestTickets(obj);
            return Ok(result);
        }


        #region Enquiry
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryComments(TicketDTO obj)
        {
            var result = service.AddEnquirycomments(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiry(TicketDTO obj)
        {
            var result = service.AddEnquiry(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryList(TicketDTO obj)
        {
            var result = service.GetEnquiryList(obj);
            return Ok(result);
        }
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewEnquiryDetails(TicketDTO obj)
        {
            var result = service.GetEnquiryDetails(obj);
            return Ok(result);
        }
        #endregion

        #region Report
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult NewCrmRawData(TicketDTO obj)
        {
            var result = service.CrmRawData(obj);
            return Ok(result);
        }
        #endregion
    }
}
