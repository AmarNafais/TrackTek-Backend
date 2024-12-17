
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface INotificationRepository
    {
        void Add(Notification notification);
        Notification GetById(int id);
        IEnumerable<Notification> GetAll();
        void Update(Notification notification);
        void Delete(int id);
    }
    public class NotificationRepository : INotificationRepository
    {
        private readonly Repository _repository;

        public NotificationRepository(Repository repository)
        {
            _repository = repository;
        }

        public void Add(Notification notification)
        {
            _repository.Notifications.Add(notification);
            _repository.SaveChanges();
        }

        public Notification GetById(int id)
        {
            return _repository.Notifications.FirstOrDefault(n => n.Id == id);
        }

        public IEnumerable<Notification> GetAll()
        {
            return _repository.Notifications.ToList();
        }

        public void Update(Notification notification)
        {
            _repository.Notifications.Update(notification);
            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var notification = _repository.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification != null)
            {
                _repository.Notifications.Remove(notification);
                _repository.SaveChanges();
            }
        }
    }
}
