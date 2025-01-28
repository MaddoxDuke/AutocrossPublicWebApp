using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

public class ContactController : Controller {
    [HttpPost]
    public IActionResult SendMessage(ContactFormModel model) {
        if (ModelState.IsValid) {
            try {
                var smtpClient = new SmtpClient("smtp.yourmailserver.com") {
                    Port = 587,
                    Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage {
                    From = new MailAddress(model.Email),
                    Subject = "Contact Form Submission",
                    Body = model.Message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add("support.autocrosssearch.org");

                smtpClient.Send(mailMessage);
                return RedirectToAction("Thank You"); // Redirect to a thank-you page
            } catch (Exception) {
                // Handle error sending email
                ModelState.AddModelError("", "Error sending message. Please try again later.");
            }
        }
        return View(model); // Show form again with error
    }
}

