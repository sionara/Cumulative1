using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;

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
            query.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";

            //create a parameter to sanitize input.
            query.Parameters.AddWithValue("@id", id);
            query.Prepare();

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

        /// <summary>
        /// Insert a new Teacher object into the teachers table in School Db.
        /// </summary>
        /// <param name="newTeacher">The Teacher object that will be inserted into the Db.</param>
        /// EXAMPLE: api/TeacherData -> New Teacher will be added to the teachers table. 
        /// Sample:
        ///<Teacher xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/Cumulative1.Models">
        ///<EmployeeNumber>sample string 4</EmployeeNumber>
        ///<HireDate>2023-12-01T20:52:34.0958606-05:00</HireDate>
        ///<Salary>6.1</Salary>
        ///<TeacherFName>sample string 2</TeacherFName>
        ///<TeacherId>1</TeacherId>
        ///<TeacherLName>sample string 3</TeacherLName>
        ///</Teacher>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")] //allow cross origin resource sharing. Allows api to share resources accessed through another domain. 
        public void AddTeacher([FromBody]Teacher newTeacher)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //validation. if any of the inputs are empty, will return to list without adding teacher.
            if (newTeacher.TeacherFName == "" || newTeacher.TeacherLName == "" || newTeacher.EmployeeNumber == "" ||
                newTeacher.HireDate.GetType() != typeof(DateTime) || newTeacher.Salary.GetType() != typeof(double)) // checking that the data type for date and salary arre correct.
            {
                return; // ends method without executing query. 
            }

            //Send appropriate query.
            query.CommandText = "INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary)" +
                "VALUES (@fname, @lname, @eNum, @hireDate, @salary)";

            query.Parameters.AddWithValue("@fname", newTeacher.TeacherFName);
            query.Parameters.AddWithValue("@lname", newTeacher.TeacherLName);
            query.Parameters.AddWithValue("@eNum", newTeacher.EmployeeNumber);
            query.Parameters.AddWithValue("@hireDate", newTeacher.HireDate);
            query.Parameters.AddWithValue("@salary", newTeacher.Salary);
            query.Prepare();

            //use this when modifying a database instead of retrieving data.
            query.ExecuteNonQuery();

            // close connection to prevent memory leak
            Conn.Close();
        }

        /// <summary>
        /// Deletes a teacher from the teachers table with teacherId = id.
        /// </summary>
        /// <param name="id">The id of the teacher to be deleted.</param>
        /// EXAMPLE POST: /api/TeacherData/{id} -> Deletes Teacher with teacherId = {id}
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //Send appropriate query.
            query.CommandText = "DELETE FROM teachers WHERE TeacherId=@key";
            query.Parameters.AddWithValue("@key", id);
            query.Prepare();

            //use this when modifying a database instead of retrieving data.
            query.ExecuteNonQuery();

            // close connection to prevent memory leak
            Conn.Close();
        }
        /// <summary>
        /// Updates an existing Teacher in school MySQL Db. Non-deterministic. This means the same input could yield different results.
        /// This is because this method may or may not work based on the existence of a teacher in the teachers table with teacherid = id.
        /// </summary>
        /// <param name="id">The id of the teacher to update</param>
        /// <param name="newTeacherInfo">The new information to update the teacher with. Received via POST request. </param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/1
        /// Form data /POST Data / Request body
        /// {
        /// "TeacherFname": sion
        /// "TeacherLname": lee
        /// "EmployeeNumber": T111
        ///  "HireDate": 12-12-23
        ///  "Salary": 50
        ///  the above will be the new info used to update teacher with teacherid = 1.
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")] //allow clients from any domain namespace to access this API. AKA CORS.
        public void UpdateTeacher(int id, [FromBody]Teacher newTeacherInfo)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //validation. if any of the inputs are empty, will return to list without adding teacher.
            if (newTeacherInfo.TeacherFName == "" || newTeacherInfo.TeacherLName == "" || newTeacherInfo.EmployeeNumber == "" ||
                newTeacherInfo.HireDate.GetType() != typeof(DateTime) || newTeacherInfo.Salary.GetType() != typeof(double)) // checking that the data type for date and salary arre correct.
            {
                return; // ends method without executing query. 
            }

            //Send appropriate query.
            query.CommandText = "UPDATE teachers SET teacherfname=@fname, teacherlname=@lname, " +
                "employeenumber=@eNum, hiredate=@hireDate, salary=@salary WHERE teacherid=@id";

            query.Parameters.AddWithValue("@fname", newTeacherInfo.TeacherFName);
            query.Parameters.AddWithValue("@lname", newTeacherInfo.TeacherLName);
            query.Parameters.AddWithValue("@eNum", newTeacherInfo.EmployeeNumber);
            query.Parameters.AddWithValue("@hireDate", newTeacherInfo.HireDate);
            query.Parameters.AddWithValue("@salary", newTeacherInfo.Salary);
            query.Parameters.AddWithValue("@id", id); // this is the id for the specific teacher coming over as a GET request in URL.
            query.Prepare();

            //use this when modifying a database instead of retrieving data.
            query.ExecuteNonQuery();

            // close connection to prevent memory leak
            Conn.Close();
        }
    }
}
