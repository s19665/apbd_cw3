using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstApi.DAL;
using FirstApi.DTOs.Requests;
using FirstApi.Modeles;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IDbService _dbService;
        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreateStudent(EnrollStudentRequest student)
        {
            Console.WriteLine("test konsoli");
            try
            {
                var enrollment = _dbService.EnrollStudent(student);
                return Created("", enrollment);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Something went wrong");
                return StatusCode(400);
            }
        }
    }
}