using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative1.Models;
using MySql.Data.MySqlClient;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        // Create new instance of SchoolDbContext class which allows connection to School Db.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a List of Teacher objects from the Teacher Table in School DB
        /// that matches the search key in their first or last name.
        /// </summary>
        /// <param name="TeacherSearchKey">Search key from Teacher/List view</param>
        /// <returns>
        /// A list of Teacher objects that matches the search key.
        /// </returns>
        /// EXAMPLE
        /// GET: api/TeacherData?TeacherSearchKey={TeacherSearchKey} -> List of Teachers
        /// GET: api/TeacherData?TeacherSearchKey=Alexander ->
        /// <Teacher>
        ///     <EmployeeNumber>T378</EmployeeNumber>
        ///     <HireDate>2016-08-05T00:00:00</HireDate>
        ///     <Salary>55.3</Salary>
        ///     <TeacherFName>Alexander</TeacherFName>
        ///     <TeacherId>1</TeacherId>
        ///     <TeacherLName>Bennett</TeacherLName>
        /// </Teacher>
        /// 
        [HttpGet]
        public List<Teacher> ListTeachers(string TeacherSearchKey)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //Send appropriate query.
            query.CommandText = "SELECT * FROM teachers WHERE lower(teacherfname) LIKE lower(@key) OR lower(teacherlname) LIKE lower(@key)";

            //parameterization to sanitize search input and prevent SQL injection
            query.Parameters.AddWithValue("@key", "%" + TeacherSearchKey + "%");
            query.Prepare();
            //Initialize a variable to store the ResultSet.
            MySqlDataReader ResultSet = query.ExecuteReader();

            //Create empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //loop through the each Row of the ResultSet and store them in Teachers list.
            while (ResultSet.Read())
            {

                // set properties of Teacher objects from Data in ResultSet
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                double Salary = Convert.ToDouble(ResultSet["salary"]);

                //create new instance of Teacher class inside the loop or it will only receive the last teacher.
                Teacher teacher = new Teacher();

                teacher.TeacherId = TeacherId;
                teacher.TeacherFName = TeacherFName;
                teacher.TeacherLName = TeacherLName;
                teacher.EmployeeNumber = EmployeeNumber;
                teacher.HireDate = HireDate;
                teacher.Salary = Salary;

                Teachers.Add(teacher);
            }

            // close connection to prevent memory leak
            Conn.Close();

            return Teachers;
        }

        /// <summary>
        /// Return a specific Teacher from the teachers DB given its ID.
        /// </summary>
        /// <param name="id">The primary key of Teacher.</param>
        /// <returns>
        /// A Teacher from the teachers DB.
        /// </returns>
        /// GET:api/TeacherData/{id} -> A Teacher Object with id of {id}.
        /// GET: api/TeacherData/1 ->
        /// <Teacher>
        ///     <EmployeeNumber>T378</EmployeeNumber>
        ///     <HireDate>2016-08-05T00:00:00</HireDate>
        ///     <Salary>55.3</Salary>
        ///     <TeacherFName>Alexander</TeacherFName>
        ///     <TeacherId>1</TeacherId>
        ///     <TeacherLName>Bennett</TeacherLName>
        /// </Teacher>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //Send appropriate query.
            query.CommandText = "SELECT * FROM teachers WHERE teacherid = " + id;

            //Initialize a variable to store the ResultSet.
            MySqlDataReader ResultSet = query.ExecuteReader();

            //create new instance of Teacher class. NO LIST
            Teacher teacher = new Teacher();

            while (ResultSet.Read())
            {

                // set properties of Teacher objects from Data in ResultSet
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                double Salary = Convert.ToDouble(ResultSet["salary"]);

                teacher.TeacherId = TeacherId;
                teacher.TeacherFName = TeacherFName;
                teacher.TeacherLName = TeacherLName;
                teacher.EmployeeNumber = EmployeeNumber;
                teacher.HireDate = HireDate;
                teacher.Salary = Salary;
            }

            // close connection to prevent memory leak
            Conn.Close();

            return teacher;
        }
    }
}
