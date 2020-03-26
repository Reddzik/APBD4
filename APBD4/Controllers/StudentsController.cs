using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBD4.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers
{
    [ApiController]
    [Route("/api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=pgago;Integrated Security=True";
        [HttpGet]
        public IActionResult getStudents()
        {
            /*var conBuilder = new SqlConnectionStringBuilder();
            conBuilder.InitialCatalog = "s18819";
*/
            var listOfStudents = new List<Student>();
            using (SqlConnection connect = new SqlConnection(ConString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from student as s, Enrollment as e, Studies as st; ";

                connect.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.NameOfStudy = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    listOfStudents.Add(st);
                }


            }
            return Ok(listOfStudents);
            /*            connect.Open();*/

        }
        [HttpGet("{IndexNumber}")]
        public IActionResult getStudent(string indexNumber)
        {
            using (SqlConnection connect = new SqlConnection(ConString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect;
                command.CommandText = "select * from Student where IndexNumber = @index";
                SqlParameter parameter = new SqlParameter();
                parameter.Value = indexNumber;
                parameter.ParameterName = "index";
                command.Parameters.Add(parameter);

                connect.Open();
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.Enrollment = dr["IdEnrollment"].ToString();
                    return Ok(st);
                }

                return NotFound();
            }


        }
    }
}