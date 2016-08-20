using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QForum.Web.Entities;
using QForum.Web.Extensions;
using QForum.Web.Mailing;
using QForum.Web.Models;
using QForum.Web.Models.AppSettings;
using QForum.Web.Storage;

namespace QForum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<Dictionary<string, string>> _values;
        private readonly IOptions<Passwords> _passwords;
        private readonly IEventInfoStorage _eventInfoStorage;
        private readonly IMailSender _mailSender;

        public HomeController(IOptions<Dictionary<string, string>> values, IOptions<Passwords> passwords, IEventInfoStorage eventInfoStorage, IMailSender mailSender)
        {
            _values = values;
            _passwords = passwords;
            _eventInfoStorage = eventInfoStorage;
            _mailSender = mailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            EventInfoEntity data = null;

            try
            {
                data = await _eventInfoStorage.GetLatestAsync();
            }
            catch (Exception ex)
            {
                // suppress
                Debug.WriteLine(ex);
            }

            if (data == null)
            {
                // set invalid values so that it doesn't show
                data = new EventInfoEntity
                {
                    RestaurantName = "",
                    RestaurantMenuUrl = "",
                    EventDate = DateTime.UtcNow.AddDays(-1)
                };
            }
            
            ViewData["RestaurantName"] = data.RestaurantName;
            ViewData["RestaurantMenuUrl"] = data.RestaurantMenuUrl;
            ViewData["NextDateReadable"] = data.EventDate.ToReadableDate();
            ViewData["HasUpcomingEvent"] = data.EventDate >= DateTime.UtcNow.AddHours(2);

            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var latestEvent = await _eventInfoStorage.GetLatestAsync();
                var subject = $"QForum {latestEvent.EventDate.ToReadableDate()}";
                var body = new StringBuilder();
                var extendedBody = new StringBuilder();

                body.AppendLine($"Namn: {model.Name}");
                body.AppendLine($"Email: {model.Email}");
                body.AppendLine($"Mat: {model.Food}");

                if (!string.IsNullOrEmpty(model.Other))
                {
                    body.AppendLine($"Annat: {model.Other}");
                }
                
                await _mailSender.SendAsync(subject, body.ToString());

                extendedBody.AppendLine("Vi ses på kontoret!");
                extendedBody.AppendLine();
                extendedBody.AppendLine("---------");
                extendedBody.AppendLine(body.ToString());

                await _mailSender.SendAsync($"Välkommen till {subject}!", extendedBody.ToString(), new List<Recipient>
                {
                    new Recipient
                    {
                        Email = model.Email,
                        Name = model.Name
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }


            return Json(new {Echo = model });
        }

        [HttpGet]
        [Route("admin/{password}")]
        public IActionResult Admin(string password)
        {
            ViewData["EnvironmentName"] = _values.Value["EnvironmentName"];

            if (!_passwords.Value.Admin.Equals(password))
                return NotFound();

            return View();
        }

        [HttpPost]
        [Route("admin/{password}/event_info")]
        public async Task<IActionResult> SaveEventInfo(string password, EventInfoModel model = null)
        {
            if (!_passwords.Value.Admin.Equals(password))
                return NotFound();

            if (model == null || !ModelState.IsValid)
                return BadRequest();

            var status = await _eventInfoStorage.InsertAsync(new EventInfoEntity(model.EventDate)
            {
                EventDate = model.EventDate,
                RestaurantMenuUrl = model.RestaurantMenuUrl,
                RestaurantName = model.RestaurantName
            });

            if(status > 299)
                throw new Exception("Something when wrong when saving to Azure Table Storage. Status = " + status);

            return Ok();
        }
    }
}
