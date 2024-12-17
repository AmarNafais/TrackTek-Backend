using Data.Entities;
using Data.Repositories;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface INotificationService
    {
        void AddNotification(CreateNotificationDTO dto);
        Notification GetNotification(int id);
        IEnumerable<Notification> GetAllNotifications();
        void UpdateNotification(UpdateNotificationDTO dto);
        void DeleteNotification(int id);
    }
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void AddNotification(CreateNotificationDTO dto)
        {
            var notification = new Notification
            {
                Message = dto.Message,
                SentDateTime = dto.SentDate
            };

            _notificationRepository.Add(notification);
        }

        public Notification GetNotification(int id)
        {
            return _notificationRepository.GetById(id);
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return _notificationRepository.GetAll();
        }

        public void UpdateNotification(UpdateNotificationDTO dto)
        {
            var notification = _notificationRepository.GetById(dto.Id);
            if (notification == null)
            {
                throw new InvalidOperationException("Notification not found.");
            }

            notification.Message = dto.Message;
            notification.SentDateTime = dto.SentDate;

            _notificationRepository.Update(notification);
        }

        public void DeleteNotification(int id)
        {
            _notificationRepository.Delete(id);
        }
    }
}
