﻿using FirstApi.DTOs.Requests;
using FirstApi.DTOs.Response;
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
        public Student GetStudentById(string id);
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest student);
        bool CheckIndex(string index);
    }
}
