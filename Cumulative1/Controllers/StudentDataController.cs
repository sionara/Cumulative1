using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative1.Models;
using MySql.Data.MySqlClient;

namespace Cumulative1.Controllers
{
    public class StudentDataController : ApiController
    {
        // Create new instance of SchoolDbContext class which allows connection to School Db.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of Students from the school Db that has first or last name matching the SearchKey.
        /// </summary>
        /// <param name="SearchKey">A string representing a search key from Student/List</param>
        /// <returns>
        /// A list of Students from the school Db who's first or last name matches the search key.
        /// </returns>
        /// GET: api/StudentData?SearchKey={SearchKey} -> A list of Students with first or last name matching {SearchKey}.
        /// GET: api/StudentData?SearchKey=sarah -> 
        /// <Student>
        ///     <EnrolDate>6/18/2018 12:00:00 AM</EnrolDate>
        ///     <Id>1</Id>
        ///     <StudentFname>Sarah</StudentFname>
        ///     <StudentLname>Valdez</StudentLname>
        ///     <StudentNumber>N1678</StudentNumber>
        /// </Student>
        [HttpGet]
        public List<Student> ListStudents(string SearchKey)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //Send appropriate query.
            query.CommandText = "SELECT * FROM students WHERE lower(studentfname) LIKE lower(@key) OR lower(studentlname) LIKE lower(@key)";

            //parameterization to sanitize search input and prevent SQL injection
            query.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            query.Prepare();
            //Initialize a variable to store the ResultSet.
            MySqlDataReader ResultSet = query.ExecuteReader();

            //Create empty list of Students
            List<Student> students = new List<Student> { };

            //loop through the each Row of the ResultSet and store them in students list.
            while (ResultSet.Read())
            {

                // set properties of student objects from Data in ResultSet
                int Id = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                string EnrolDate = ResultSet["enroldate"].ToString();
                
                //create new instance of Student class inside the loop or it will only receive the last student.
                Student student = new Student();

                student.Id = Id;
                student.StudentFname = StudentFname;
                student.StudentLname = StudentLname;
                student.StudentNumber = StudentNumber;
                student.EnrolDate = EnrolDate;

                students.Add(student);
            }

            // close connection to prevent memory leak
            Conn.Close();

            return students;
        }

        /// <summary>
        /// Return a specific Student from the Students DB given its ID.
        /// </summary>
        /// <param name="id">The primary key of Student.</param>
        /// <returns>
        /// A Student from the Students DB.
        /// </returns>
        /// GET: api/StudentData/{id} -> A Student with id of {id}
        /// GET: api/StudentData/1 ->
        /// <Student>
        ///     <EnrolDate>6/18/2018 12:00:00 AM</EnrolDate>
        ///     <Id>1</Id>
        ///     <StudentFname>Sarah</StudentFname>
        ///     <StudentLname>Valdez</StudentLname>
        ///     <StudentNumber>N1678</StudentNumber>
        /// </Student>

        [HttpGet]
        public Student FindStudent(int id)
        {
            // Initialize connection to DB.
            MySqlConnection Conn = School.AccessDatabase();

            //open connection.
            Conn.Open();

            //create variable to represent SQL query
            MySqlCommand query = Conn.CreateCommand();

            //Send appropriate query.
            query.CommandText = "SELECT * FROM students WHERE studentid = @id";

            //create a parameter to preven SQL injection
            query.Parameters.AddWithValue("@id", id);
            query.Prepare();

            //Initialize a variable to store the ResultSet.
            MySqlDataReader ResultSet = query.ExecuteReader();

            //create new instance of Student class. NO LIST
            Student Student = new Student();

            while (ResultSet.Read())
            {

                // set properties of Student objects from Data in ResultSet
                int Id = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                string EnrolDate = ResultSet["enroldate"].ToString();

                Student.Id = Id;
                Student.StudentFname = StudentFname;
                Student.StudentLname = StudentLname;
                Student.StudentNumber = StudentNumber;
                Student.EnrolDate = EnrolDate;
            }

            // close connection to prevent memory leak
            Conn.Close();

            return Student;
        }
    }
}
