using Newtonsoft.Json;
using PVMSClientApp.Models;
using PVMSClientApp.Models.DAO;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PVMSClientApp.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services

        public ActionResult Index()
        {
            string userId = Session["userId"].ToString();
            List<string> ids = new List<string>();
            using (var client2 = new HttpClient())
            {
                client2.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTask = client2.GetAsync("Services/UpdateStatus?userId=" + userId);
                responseTask.Wait();
                var result2 = responseTask.Result;
                var readData = result2.Content.ReadAsStringAsync().Result;
                if (result2.IsSuccessStatusCode)
                {
                    ids = JsonConvert.DeserializeObject<List<string>>(readData);
                    Session["passportId"] = ids[0];
                    Session["visaId"] = ids[1];
                }
                else
                {
                    return View();
                }
            }
            return View();
        }



        [HttpPost]
        public ActionResult Index(string btn)
        {
            string userId = Session["userId"].ToString();


            if (btn.Equals("APPLY PASSPORT"))

            {
                ViewBag.msg1 = null;
                ViewBag.msg2 = null;
                using (var client2 = new HttpClient())
                {
                    client2.BaseAddress = new Uri("https://localhost:44386/api/");
                    var responseTask = client2.GetAsync("Services/ApplyPassport?userId="+userId);
                    responseTask.Wait();
                    var result2 = responseTask.Result;
                    var readData = result2.Content.ReadAsAsync<passport>();
                    if (result2.IsSuccessStatusCode)
                    {
                        ViewBag.msg = "True";
                        ViewBag.msg1 = "You Already Have a Valid Passport.";
                        ViewBag.msg2 = "You can apply for Paassport Renewal.";
                        return View();
                    }
                }
                return RedirectToAction("PassportRegistration", "PassportReg");
            }
            else if (btn.Equals("APPLY VISA"))

            {
                ViewBag.msg1 = null;
                ViewBag.msg2 = null;
                passport p = null;
                using (var client2 = new HttpClient())
                {
                    client2.BaseAddress = new Uri("https://localhost:44386/api/");
                    var responseTask = client2.GetAsync("Services/ApplyVisa?userId="+userId);
                    responseTask.Wait();
                    var result2 = responseTask.Result;
                    var readData = result2.Content.ReadAsAsync<passport>();
                    if (result2.IsSuccessStatusCode)
                    {
                        p = readData.Result;
                        if(p==null)
                        {
                            ViewBag.msg = "True";
                            ViewBag.msg1 = "You may not have a valid Passport.";
                            ViewBag.msg2 = "Apply a new one or renewal the old one.";
                            return View();
                        }
                    }
                }
                return RedirectToAction("VisaRegistration", "VisaReg");
            }
            else if (btn.Equals("PASSPORT RENEWAL"))
            {
                ViewBag.msg1 = null;
                ViewBag.msg2 = null;
                List<passport> passports = null;
                using (var client2 = new HttpClient())
                {
                    client2.BaseAddress = new Uri("https://localhost:44386/api/");
                    var responseTask = client2.GetAsync("Services/PassportRenewal?userId="+userId);
                    responseTask.Wait();
                    var result2 = responseTask.Result;
                    var readData = result2.Content.ReadAsStringAsync().Result;
                    if (result2.IsSuccessStatusCode)
                    {
                        passports = JsonConvert.DeserializeObject<List<passport>>(readData);
                        if (passports.Count == 0)
                        {
                            ViewBag.msg = "True";
                            ViewBag.msg1 = "You may not have a valid Passport to Renew it.";
                            ViewBag.msg2 = "You can Apply for one";
                            return View();
                        }
                        
                    }
                    
                }
                return RedirectToAction("Renewal", "PassportRenewal");
            }
            else
            {
                ViewBag.msg1 = null;
                ViewBag.msg2 = null;

                List<visa> visas = null;
                using (var client2 = new HttpClient())
                {
                    client2.BaseAddress = new Uri("https://localhost:44386/api/");
                    var responseTask = client2.GetAsync("Services/VisaCancellation?userId="+userId);
                    responseTask.Wait();
                    var result2 = responseTask.Result;
                    var readData = result2.Content.ReadAsStringAsync().Result;
                    if (result2.IsSuccessStatusCode)
                    {
                        visas = JsonConvert.DeserializeObject<List<visa>>(readData);
                        if (visas.Count == 0)
                        {
                            ViewBag.msg = "True";
                            ViewBag.msg1 = "You may not have a valid Visa to cancel.";
                            ViewBag.msg2 = "Apply for a Visa";

                            return View();
                        }
                        
                    }
                    
                }
                return RedirectToAction("VisaCancellation", "VisaCancellation");
            }
        }

        public ActionResult UserProfile()
        {
            ViewBag.passportId = Session["passportId"].ToString();
            ViewBag.visaId = Session["visaId"].ToString();
            string userId = Session["userId"].ToString();
            ViewBag.userId = Session["userId"].ToString();
            userProfile user = null;
            using (var client2 = new HttpClient())
            {
                client2.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTask = client2.GetAsync("Services/GetUser?userId="+userId);
                responseTask.Wait();
                var result2 = responseTask.Result;
                var readData = result2.Content.ReadAsAsync<userProfile>();
                if (result2.IsSuccessStatusCode)
                {
                    user = readData.Result;
                    return View(user);
                }
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult UserProfile(userProfile user)
        {
            ViewBag.passportId = Session["passportId"].ToString();
            ViewBag.visaId = Session["visaId"].ToString();
            ViewBag.userId = Session["userId"].ToString();
            user.userId = Session["userId"].ToString();
            using (var client2 = new HttpClient())
            {
                client2.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTask = client2.PostAsJsonAsync<userProfile>("Services/UpdateUser", user);
                responseTask.Wait();
                var result2 = responseTask.Result;
                var readData = result2.Content.ReadAsAsync<userProfile>();
                if (result2.IsSuccessStatusCode)
                {
                    user = readData.Result;
                    return View(user);
                }
            }
            return View(user);
        }

        public ActionResult GeneratePDF()
        {
            var pdf = new ViewAsPdf("../Services/IdCard")
            {
                PageSize = Size.A4,
                FileName = "PassportId.pdf"
            };

            return pdf;
        }

        public ActionResult IdCard()
        {
            PassportId p = new PassportId();
            p.type = "P";
            p.countryCode = "IND";
            p.nationality = "INDIAN";
            p.placeOfIssue = "HYDERABAD";
            string userId = Session["userId"].ToString();
            using (var client2 = new HttpClient())
            {
                client2.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTask = client2.GetAsync("Services/GetPassportId?userId="+userId);
                responseTask.Wait();
                var result2 = responseTask.Result;
                var readData = result2.Content.ReadAsAsync<PassportId>();
                if (result2.IsSuccessStatusCode)
                {
                    p = readData.Result;
                    return View(p);
                }
            }
            
            p.passportNo = "FPS-30001";
            p.surname = "VARUN";
            p.givenName = "CHAKRAVARTHY";
            p.gender = "M";
            p.dob = "12/10/1980";
            p.placeOfBirth = "WARANGAL";
            p.doi = "11/10/2021";
            p.doe = "11/10/2031";

            return View(p);
        }
    }
}