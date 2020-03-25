using FirstApi.Modeles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DAL
{
    public class MssqlDbService : IDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s19665;Integrated Security=True";
        public IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>();
            var conBulder = new SqlConnectionStringBuilder();
            conBulder.InitialCatalog = "s19665";
            string conStr = conBulder.ConnectionString;
            using (var client = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText =
                    "Select " +
                    "FirstName, LastName, BirthDate, Name, Semester " +
                    "from Student s join Enrollment e on s.idEnrollment=e.idEnrollment " +
                    "join Studies st on st.IdStudy=e.IdStudy;";

                client.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.Study = dr["Name"].ToString();
                    st.Semester = int.Parse(dr["Semester"].ToString());
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    students.Add(st);
                }
            }
            return students;
        }

        public Student GetStudentById(string id)
        {
            var conBulder = new SqlConnectionStringBuilder();
            conBulder.InitialCatalog = "s19665";
            string conStr = conBulder.ConnectionString;
            using (var client = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText =
                    "Select " +
                    "FirstName, LastName, BirthDate, Name, Semester " +
                    "from Student s join Enrollment e on s.idEnrollment=e.idEnrollment " +
                    "join Studies st on st.IdStudy=e.IdStudy where s.IndexNumber=@id;";
                com.Parameters.AddWithValue("id", id);
                client.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.Study = dr["Name"].ToString();
                    st.IndexNumber = id;
                    st.Semester = int.Parse(dr["Semester"].ToString());
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    return st;
                }
            }
            return null;
        }
    }
}
