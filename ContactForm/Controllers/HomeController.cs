using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using System.Net;
using ContactForm.Models;

namespace ContactForm.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostForm(string ContactName, string CompanyName, string Address, string City, string State, string Zip, string Phone, string Email, string RequestText, string Captcha)
        {
            if (ModelState.IsValid)
            {
                if (Session["Captcha"].ToString() != Captcha)
                {
                    ResetSetCaptchaText();
                    return View("Index");
                }
                else
                {
                    string Msg = BuildMsg(ContactName, CompanyName, Address, City, State, Zip, Phone, Email, RequestText);
                    string CustomerMsg = BuildCustomerMsg();
                    bool EmailSent = false;
                    Stream[] stream = new Stream[Request.Files.Count]; 
                    try
                    {
                        string[] fileName = new string[Request.Files.Count];
                        int count = 0;
                        foreach (string file in Request.Files)
                        {
                            var fileContent = Request.Files[file];
                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                fileName[count] = fileContent.FileName;
                                string[] split = fileName[count].Split('\\');
                                fileName[count] = split[split.Length - 1];
                                stream[count] = fileContent.InputStream;
                                count++;
                            }
                        }
                        // send email
                        EmailSent = Classes.Email.SendEmail("chae.song20@gmail.com", "Rhinestone Empire - Contact Form", Msg, stream, fileName, false);
                        //EmailSent = Classes.Email.SendEmail("steve.yi@gmail.com", "Rhinestone Empire - Contact Form", Msg, stream, fileName, false);
                        if (EmailSent == true)
                        {
                            try
                            {
                                EmailSent = Classes.Email.SendEmail(Email, "Rhinestone Empire - Contact Form", CustomerMsg, stream, fileName, true);
                            }
                            catch
                            {
                                return Content("Failed");
                            }
                        }
                    }
                    catch
                    {
                        return Content("Failed");
                    }
                    return Content("Success");
                }
            }
            else
            {
                //ModelState.AddModelError("", "Email Address or Password is Incorrect.");
            }

            return View("Index");
        }

        public string BuildMsg(string ContactName, string CompanyName, string Address, string City, string State, string Zip, string Phone, string Email, string RequestText)
        {
            string ToReturn = "<br/>Contact Name: " + ContactName + "<br/>" + "Company Name: " + CompanyName + "<br/>" + "Address: " + Address + "<br/>" +
                   "City: " + City + "<br/>" + "State: " + State + "<br/>" + "Zip: " + Zip + "<br/>" + "Phone: " + Phone + "<br/>" + "Email: " + Email + "<br/>" + "Request: " + RequestText + "<br/>";
            return ToReturn;
        }

        public string BuildCustomerMsg()
        {
            string ToReturn = "Thank you for submitting your form! Rhinestone Empire will contact you shortly.<br/><br>";
            return ToReturn;
        }

        [HttpPost]
        public JsonResult ResetSetCaptchaText()
        {
            Random oRandom = new Random();
            int iNumber = oRandom.Next(100000, 999999);
            Session["Captcha"] = iNumber.ToString();
            return Json(iNumber);
        }

        [HttpPost]
        public JsonResult ReturnSession()
        {
            return Json(Convert.ToInt32(Session["Captcha"]));
        }

    }
}
