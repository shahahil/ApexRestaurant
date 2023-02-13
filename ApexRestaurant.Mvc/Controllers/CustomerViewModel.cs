using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ApexRestaurant.Mvc.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
public class CustomerController : Controller
{
    private string? baseUri;
    public CustomerController(IConfiguration iConfig)
    {
        // Get baseUri of Web API from appsettings.json > ApiBaseUrl
       String? baseUri = iConfig.GetValue<string>("ApiBaseUrl");
    }
    // GET: Customer
    public ActionResult Index()
    {
        IEnumerable<CustomerViewModel> customers = null;
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Add("accept", "application/json");
            var responseTask = client.GetAsync("api/customer");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var apiResponse = result.Content.ReadAsStringAsync();
                apiResponse.Wait();
                customers =
                JsonConvert.DeserializeObject<IList<CustomerViewModel>>(apiResponse.Result);
            }
            else //web api sent error response
            {
                customers = Enumerable.Empty<CustomerViewModel>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact
                administrator.");
            }
        }
        return View(customers);
    }
}