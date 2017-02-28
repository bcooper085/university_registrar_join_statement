using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Student
  {
      private int _id;
      private string _name;
      private DateTime _enrollmentDate;

      public Student(string Name, DateTime EnrollmentDate, int Id = 0)
      {
        _id = Id;
        _name = Name;
        _enrollmentDate = EnrollmentDate;
      }
      public int GetId()
      {
        return _id;
      }

      public string GetName()
      {
        return _name;
      }

      public DateTime GetEnrollmentDate()
      {
        return _enrollmentDate;
      }

      public override bool Equals(System.Object otherStudent)
      {
        if (!(otherStudent is Student))
        {
          return false;
        }
        else
        {
          Student newStudent = (Student) otherStudent;
          bool idEquality = this.GetId() == newStudent.GetId();
          bool nameEquality = this.GetName() == newStudent.GetName();
          bool enrollmentDateEquality = this.GetEnrollmentDate() == newStudent.GetEnrollmentDate();
          return (idEquality && nameEquality && enrollmentDateEquality);
        }
      }
      public static List<Student> GetAll()
      {
        List<Student> allStudents = new List<Student>{};

        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          int studentId = rdr.GetInt32(0);
          string studentName = rdr.GetString(1);
          DateTime studentEnrollmentDate = rdr.GetDateTime(2);
          Student newStudent = new Student(studentName, studentEnrollmentDate, studentId);
          allStudents.Add(newStudent);
        }

        if (rdr != null)
        {
          rdr.Close();
        }
        if (conn != null)
        {
          conn.Close();
        }

        return allStudents;
      }

      public void Save()
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO students (name, enrollment_date) OUTPUT INSERTED.id VALUES (@StudentName, @StudentEnrollmentDate);", conn);

        SqlParameter nameParameter = new SqlParameter();
        nameParameter.ParameterName = "@StudentName";
        nameParameter.Value = this.GetName();
        cmd.Parameters.Add(nameParameter);

        SqlParameter studentEnrollmentDateParameter = new SqlParameter();
        studentEnrollmentDateParameter.ParameterName = "@StudentEnrollmentDate";
        studentEnrollmentDateParameter.Value = this.GetEnrollmentDate();
        cmd.Parameters.Add(studentEnrollmentDateParameter);
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          this._id = rdr.GetInt32(0);
        }
        if (rdr != null)
        {
          rdr.Close();
        }
        if(conn != null)
        {
          conn.Close();
        }
      }

      public static Student Find(int id)
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
        SqlParameter studentIdParameter = new SqlParameter();
        studentIdParameter.ParameterName = "@StudentId";
        studentIdParameter.Value = id.ToString();
        cmd.Parameters.Add(studentIdParameter);
        SqlDataReader rdr = cmd.ExecuteReader();

        int foundStudentId = 0;
        string foundStudentName = null;
        DateTime foundStudentEnrollmentDate = new DateTime();

        while(rdr.Read())
        {
          foundStudentId = rdr.GetInt32(0);
          foundStudentName = rdr.GetString(1);
          foundStudentEnrollmentDate = rdr.GetDateTime(2);
        }
        Student foundStudent = new Student(foundStudentName, foundStudentEnrollmentDate, foundStudentId);

        if (rdr != null)
        {
          rdr.Close();
        }
        if (conn != null)
        {
          conn.Close();
        }
        return foundStudent;
      }

      public void AddCourse(Course newCourse)
      {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);", conn);
          SqlParameter courseIdParameter = new SqlParameter();
          courseIdParameter.ParameterName = "@CourseId";
          courseIdParameter.Value = newCourse.GetId();
          cmd.Parameters.Add(courseIdParameter);

          SqlParameter studentIdParameter = new SqlParameter();
          studentIdParameter.ParameterName = "@StudentId";
          studentIdParameter.Value = this.GetId();
          cmd.Parameters.Add(studentIdParameter);

          cmd.ExecuteNonQuery();

          if (conn != null)
          {
              conn.Close();
          }
      }

      public List<Course> GetCourses()
      {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN courses_students ON (students.id = courses_students.student_id) JOIN courses ON (courses_students.course_id = courses.id) WHERE students.id = @StudentId", conn);
          SqlParameter StudentIdParameter = new SqlParameter();
          StudentIdParameter.ParameterName = "@StudentId";
          StudentIdParameter.Value = this.GetId().ToString();

          cmd.Parameters.Add(StudentIdParameter);

          SqlDataReader rdr = cmd.ExecuteReader();

          List<Course> courses = new List<Course>{};

          while(rdr.Read())
          {
            int courseThisId = rdr.GetInt32(0);
            string courseName = rdr.GetString(1);
            string courseNumber = rdr.GetString(2);
            Course foundCourse = new Course(courseName, courseNumber, courseThisId);
            courses.Add(foundCourse);
          }
          if (rdr != null)
          {
              rdr.Close();
          }
          if (conn != null)
          {
              conn.Close();
          }
          return courses;
      }

      public void Delete()
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM courses_students WHERE student_id = @StudentId;", conn);
        SqlParameter studentIdParameter = new SqlParameter();
        studentIdParameter.ParameterName = "@StudentId";
        studentIdParameter.Value = this.GetId();

        cmd.Parameters.Add(studentIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }

      public static void DeleteAll()
      {
        SqlConnection conn = DB.Connection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
        cmd.ExecuteNonQuery();
        conn.Close();
      }
    }
  }
