using FirstApi.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;
        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IdStudent="s1", FirstName="Jan", LastName="Kowalski"},
                new Student{IdStudent="s2", FirstName="Anna", LastName="Malewski"},
                new Student{IdStudent="s3", FirstName="Andrzej", LastName="Andrzejewicz"}
            };
        }

        public Student GetStudentById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
