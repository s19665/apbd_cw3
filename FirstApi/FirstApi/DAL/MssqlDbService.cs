using FirstApi.DTOs.Requests;
using FirstApi.DTOs.Response;
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

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest student)
        {
            using (var conn = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                conn.Open();
                SqlTransaction tr = conn.BeginTransaction();
                try
                {
                    com.Transaction = tr;
                    com.Connection = conn;

                    // Czy studia istnieja?
                    com.CommandText = "select IdStudy from studies where name=@name";
                    com.Parameters.AddWithValue("name", student.Studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tr.Rollback();
                        throw new Exception("Studia nie istnieja");
                    }
                    int idstudies = (int)dr["IdStudy"];
                    dr.Close();

                    com.CommandText = $"select idEnrollment from Enrollment where idStudy = @id and Semester = 1";
                    com.Parameters.AddWithValue("id", idstudies);
                    dr = com.ExecuteReader();
                    int semId = -1;
                    while (dr.Read())
                    {
                        semId = (int)dr.GetValue(0);

                    }

                    int maxid = 4;
                    // dodanie nowego semestru jeżeli nie istenije
                    if (semId == -1)
                    {
                        dr.Close();
                        com.CommandText = $"select max(idEnrollment)+1 from Enrollment";
                        dr = com.ExecuteReader();
                        while (dr.Read())
                        {
                            maxid = (int)dr.GetValue(0);

                        }
                        DateTime date = DateTime.Now;
                        dr.Close();
                        com.CommandText = $"insert into Enrollment values (@idEnrollment, 1, @idStudy, @StartDate)";
                        com.Parameters.AddWithValue("idEnrollment", maxid);
                        com.Parameters.AddWithValue("idStudy", idstudies);
                        com.Parameters.AddWithValue("StartDate", date);
                        com.ExecuteNonQuery();
                    }

                    dr.Close();
                    //dodanie studenta
                    //com.CommandText = $"insert into Student values (@id, @firstname, @lastname, @enrollment)";
                    com.CommandText = $"insert into Student values (@id_2, @firstname_2, @lastname_2, @birthdate_2, @enrollment_2)";
                    com.Parameters.AddWithValue("id_2", student.IndexNumber.ToString());
                    com.Parameters.AddWithValue("firstname_2", student.FirstName);
                    com.Parameters.AddWithValue("lastname_2", student.LastName);
                    com.Parameters.AddWithValue("birthdate_2", student.BirthDate);
                    com.Parameters.AddWithValue("enrollment_2", maxid);
                    com.ExecuteNonQuery();
                    tr.Commit();

                    return new EnrollStudentResponse
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Semester = 1,
                        StartDate = DateTime.Now
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    tr.Rollback();
                }
                throw new Exception("Something went wrong");
            }
        }
    }
}
