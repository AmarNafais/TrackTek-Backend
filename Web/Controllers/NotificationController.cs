using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service;
using Data.Entities;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Route("v1/Notification/Create")]
        [HttpPost]
        public IActionResult CreateNotification(CreateNotificationDTO dTO)
        {
            try
            {
                _notificationService.AddNotification(dTO);
                return Ok("Notification created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Notification/GetById")]
        [HttpGet]
        public IActionResult GetNotificationById(int id)
        {
            try
            {
                var notification = _notificationService.GetNotification(id);
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Notification/GetAll")]
        [HttpGet]
        public IActionResult GetAllNotifications()
        {
            try
            {
                var notifications = _notificationService.GetAllNotifications();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Notification/Update")]
        [HttpPut]
        public IActionResult UpdateNotification(UpdateNotificationDTO dTO)
        {
            try
            {
                _notificationService.UpdateNotification(dTO);
                return Ok("Updated Notification Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Notification/Delete")]
        [HttpDelete]
        public IActionResult DeleteNotification(int id)
        {
            try
            {
                _notificationService.DeleteNotification(id);
                return Ok("Deleted Notification Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
