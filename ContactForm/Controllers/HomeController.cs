using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactForm;
using System.IO;
using System.Collections;
using ContactForm.Models;
using System.Web.Services;

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
        public ActionResult PostForm(string ContactName, string CompanyName, string Address, string City, string State, string Zip, string Phone, string Email, string Request, string Captcha)
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
                    string Msg = BuildMsg(ContactName, CompanyName, Address, City, State, Zip, Phone, Email, Request);
                    string CustomerMsg = BuildCustomerMsg();
                    bool EmailSent = false;
                    try
                    {
                        // send email
                        EmailSent = Classes.Email.SendEmail("chae.song20@gmail.com", "Rhinestone Empire - Contact Form", Msg);
                        //EmailSent = Classes.Email.SendEmail("steve.yi@gmail.com", "Rhinestone Empire - Contact Form", Msg);
                        if (EmailSent == true)
                        {
                            try
                            {
                                EmailSent = Classes.Email.SendEmail(Email, "Rhinestone Empire - Contact Form", CustomerMsg);
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

        public string BuildMsg(string ContactName, string CompanyName, string Address, string City, string State, string Zip, string Phone, string Email, string Request)
        {
            string ToReturn = "<br/>Contact Name: " + ContactName + "<br/>" + "Company Name: " + CompanyName + "<br/>" + "Address: " + Address + "<br/>" +
                   "City: " + City + "<br/>" + "State: " + State + "<br/>" + "Zip: " + Zip + "<br/>" + "Phone: " + Phone + "<br/>" + "Email: " + Email + "<br/>" + "Request: " + Request + "<br/>";
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
