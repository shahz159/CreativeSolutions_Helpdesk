using HelpDesk.Web.Handlers;
using HelpDesk.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Web.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Product
        public async Task<ActionResult> Product()
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        ProductDTO obj = new ProductDTO();

                        long userid = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        obj.CompanyId = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.CompanyId = 10;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ProductAPI/NewGetProductsList", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<List<ProductDTO>>(responseData);
                            if (categories.Count != 0)
                                obj.ProductsLst = categories;
                            else
                                obj.ProductsLst = null;
                        }
                        return View(obj);
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }
        public ActionResult Create()
        {
            ProductDTO obj = new ProductDTO();
            if (TempData["obj"] != null)
            {
                obj = (ProductDTO)TempData["obj"];  //retrieve TempData values here
                ViewData["Submit"] = "false";
                ViewData["Update"] = "true";
            }
            else
            {
                ViewData["Submit"] = "true";
                ViewData["Update"] = "false";
                obj.ProductId = 0;
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductDTO obj, string Submit, string Update)
        {
            string ses = Convert.ToString(Session["SSUserId"]);
            if (string.IsNullOrEmpty(ses))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    CommonHeader.setHeaders(client);
                    try
                    {
                        if (Submit == "Submit")
                            obj.FlagId = 1;
                        if (Update == "Update")
                            obj.FlagId = 2;
                        obj.CreatedBy = long.Parse(Session["SSUserId"].ToString());
                        int roleid = int.Parse(Session["SSRoleId"].ToString());
                        obj.CompanyId = int.Parse(Session["SSCompanyId"].ToString());
                        int orgid = int.Parse(Session["SSOrganizationId"].ToString());
                        obj.CompanyId = 10;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ProductAPI/NewInsertUpdateProducts", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var categories = JsonConvert.DeserializeObject<ProductDTO>(responseData);
                            obj.message = categories.message;
                            string msg = obj.message;
                        }
                        return RedirectToAction("Product");
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                CommonHeader.setHeaders(client);
                try
                {
                    ProductDTO a = new ProductDTO();
                    a.ProductId = id;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ProductAPI/NewGetProductById", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var user = JsonConvert.DeserializeObject<ProductDTO>(responseData);
                        TempData["obj"] = user;
                        return RedirectToAction("Create");
                    }
                    return View("Error");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }
    }
}