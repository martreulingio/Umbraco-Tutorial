using System;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using CleanBlog.Core.ViewModels;
using System.Web;
using Umbraco.Web;
using System.Net.Mail;
using Umbraco.Core.Logging;
using CleanBlog.Core.Services;

namespace CleanBlog.Core.Controllers
{

    public class ContactSurfaceController : SurfaceController
    {
        private readonly ISmtpService _smtpService;
        public ContactSurfaceController(ISmtpService smtpService)
        {
            _smtpService = smtpService;
        }
        [HttpGet]
        public ActionResult RenderForm()
        {
            ContactViewModel model = new ContactViewModel() { ContactFormId = CurrentPage.Id };
            return PartialView("~/Views/Partials/Contact/ContactForm.cshtml", model);
        }
        [HttpPost]
        public ActionResult RenderForm(ContactViewModel model)
        {
            return PartialView("~/Views/Partials/Contact/ContactForm.cshtml", model);
        }

        public ActionResult SubmitForm(ContactViewModel model)
        {
            bool success = false;

            if (ModelState.IsValid)
            {
                success = _smtpService.SendEmail(model);
            }

            var contactPage = UmbracoContext.Content.GetById(false, model.ContactFormId);

            var successMessage = contactPage.Value<IHtmlString>("succesMessage");
            var errorMessage = contactPage.Value<IHtmlString>("errorMessage");

            return PartialView("~/Views/Partials/Contact/Result.cshtml", success ? successMessage : errorMessage);
        }

        
    }
}
