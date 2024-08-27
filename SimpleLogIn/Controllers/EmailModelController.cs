using Microsoft.AspNetCore.Mvc;
using SimpleLogIn.Data;
using SimpleLogIn.Models;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Session;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using SimpleLogIn.Repository.IRepository;

namespace SimpleLogIn.Controllers
{
    public class EmailModelController : Controller
    {

        private readonly IEmailModelRepository _emailrepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public EmailModelController(IEmailModelRepository dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _emailrepository = dbContext;
            _WebHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {

            List<EmailModel> emailList = _emailrepository.GetAllUser().ToList();
            return View(emailList);
        }

       
        public IActionResult Create()
        {
            EmailModelView vm = new()
            {
                EmailModel = new EmailModel()
            };
            return View();
        }

        [HttpPost]
       
        public IActionResult Create(EmailModelView obj, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
               
                var check = _emailrepository.GetAUser(u=>u.FullName == obj.EmailModel.FullName || u.UserEmail== obj.EmailModel.UserEmail);
                if (check != null) 
                {
                    TempData["error"] = "Email already exist";
                    return View(obj);
                }
                else
                {
                    string wwwRootPath = _WebHostEnvironment.WebRootPath;

                    if (imageUrl != null && IsImageFile(imageUrl.FileName))
                    {
                       string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl.FileName);//naming the files and its extension.


                       string productPath = Path.Combine(wwwRootPath, @"Images\Photos"); //location to save file 


                       using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                       {
                            imageUrl.CopyTo(fileStream);
                       }

                             
                       obj.EmailModel.ImageUrl= @"\Images\Photos\" + fileName;
                    }

                    try
                    {


                        _emailrepository.AddAUser(obj.EmailModel);
                       SendEmailNotification(obj);
                        _emailrepository.Save();

                        return RedirectToAction("Index");

                    }
                    catch (Exception)
                    {

                        TempData["error"] = "Invalid file format.Please enter a valid image file!";
                    }
                }
            }

            

            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
         public IActionResult Login(EmailModel objEmailModel) 
         {
            if (ModelState.IsValid)
            {
                var user = _emailrepository.GetAUser(u => u.FullName == objEmailModel.FullName && u.UserEmail == objEmailModel.UserEmail && u.Password == objEmailModel.Password);  
                
                if(user != null)
                {
                    

                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("UserEmail", user.UserEmail);
                    HttpContext.Session.SetString("password", user.Password);
                    HttpContext.Session.SetString("ImageUrl", user.ImageUrl);
                   

                    return RedirectToAction("Dashboard");
                }

                else
                {
                    
                    TempData["error"] = "Invalid Credentials";
                    return View(objEmailModel);
                }
            }

            return RedirectToAction("Index");
         }

        public IActionResult Dashboard()
        {
        
          ViewBag.FullName = HttpContext.Session.GetString("FullName");
            ViewBag.ImageUrl = HttpContext.Session.GetString("ImageUrl");

            return View();
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Remove("FullName");

            return RedirectToAction("Index");
        }


        [NonAction]
        private bool IsImageFile(string fileName)
        {
                bool isValid = false;
                string[] fileExtensions = { ".bmp", ".jpg", ".png", ".gif", ".jpeg", ".BMP", ".JPG", ".PNG", ".GIF", ".JPEG" };

           for (int i = 0; i < fileExtensions.Length; i++)
           {
                 if (fileName.Contains(fileExtensions[i]))
                 {
                     isValid = true;
                 }
           }
              return isValid;  
        }


        private void SendEmailNotification(EmailModelView user)
        {
            try
            {
                // Set up the SMTP client with your email server details
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("kadwales@gmail.com", "iokioioii");
                    smtpClient.EnableSsl = true; // Use SSL if required

                    // Create the email message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("kadwales@gmail.com");
                    mailMessage.To.Add(user.EmailModel.UserEmail); // Replace with the new user's email
                    mailMessage.Subject = "Welcome to my simple login Website";
                    mailMessage.Body = "Dear " + user.EmailModel.FullName + ",\n\nWelcome to our website!";

                    // Send the email
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception)
            {
                //this is not necessary because if the user's email doesnt exist the smtp provider will send an error message to that effect
                TempData["errorEmail"] = "Email doe not exist";
            }
        }
    }
    
}
