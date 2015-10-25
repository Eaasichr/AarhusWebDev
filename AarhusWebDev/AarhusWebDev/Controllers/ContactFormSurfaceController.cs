using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDev.ViewModels;
using Umbraco.Core.Models;

namespace AarhusWebDev.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            {
                // sender mail
                MailMessage message = new MailMessage();
                message.To.Add("smonchristensen17@gmail.com");
                message.Subject = model.Subject;
                message.From = new MailAddress(model.Email, model.Name);
                message.Body = model.Message + "\n my email is: " + model.Email;
                
                TempData["success"] = true;

                IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Comments");

                comment.SetValue("name", model.Name);
                comment.SetValue("email", model.Email);
                comment.SetValue("subject", model.Subject);
                comment.SetValue("message", model.Message);

                // Save
                Services.ContentService.Save(comment);

                //Save and publish
                //Services.ContentService.SaveAndPublishWithStatus(comment);

                return RedirectToCurrentUmbracoPage();
            }

            

        }
    }
}
