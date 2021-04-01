using Kafka.Domain.Entities;
using Kafka.Domain.Interfaces;

namespace Kafka.Infra.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository()
        {

        }
    }
}
