using Data.Entities;

namespace Data.Repositories
{
    public interface IGarmentRepository
    {
        long Add(Garment garment);
        Garment GetById(int id);
        IEnumerable<Garment> GetAll();
        void Update(Garment garment);
        void Delete(int id);
    }

    public class GarmentRepository : IGarmentRepository
    {
        private readonly Repository _repository;

        public GarmentRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(Garment garment)
        {
            var existingGarment = _repository.Garments.Any(g => g.Name == garment.Name);
            if (existingGarment)
            {
                throw new InvalidOperationException("Garment already Exists");
            }

            _repository.Garments.Add(garment);
            _repository.SaveChanges();
            return garment.Id;
        }

        public Garment GetById(int id)
        {
            return _repository.Garments.FirstOrDefault(g => g.Id == id)
                ?? throw new InvalidOperationException($"Garment with ID {id} not found.");
        }

        public IEnumerable<Garment> GetAll()
        {
            return _repository.Garments;
        }

        public void Update(Garment garment)
        {
            var garmentToUpdate = _repository.Garments.FirstOrDefault(g => g.Id == garment.Id)
                ?? throw new InvalidOperationException($"Garment with ID {garment.Id} not found.");

            garmentToUpdate.Name = garment.Name;
            garmentToUpdate.Design = garment.Design;
            garmentToUpdate.CategoryType = garment.CategoryType;
            garmentToUpdate.Sizes = garment.Sizes;
            garmentToUpdate.BasePrice = garment.BasePrice;
            garmentToUpdate.GarmentStatus = garment.GarmentStatus;
            garmentToUpdate.LaborHoursPerUnit = garment.LaborHoursPerUnit;
            garmentToUpdate.HourlyLaborRate = garment.HourlyLaborRate;

            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var garmentToUpdate = _repository.Garments.FirstOrDefault(g => g.Id == id)
                ?? throw new InvalidOperationException($"Garment with ID {id} not found.");
            _repository.Garments.Remove(garmentToUpdate);
            _repository.SaveChanges();
        }
    }
}
