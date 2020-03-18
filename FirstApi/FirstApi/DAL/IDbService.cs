using FirstApi.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
