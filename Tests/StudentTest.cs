using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test ;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_StudentDatabaseEmptyOnLoad_Empty()
    {
      int result = Student.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Student testStudent = new Student("Billy", date1);
      testStudent.Save();

      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Student testStudent = new Student("Billy", date1);
      testStudent.Save();

      //Act
      Student savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindsStudentInDatabase()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Student testStudent = new Student("Billy", date1);
      testStudent.Save();

      //Act
      Student foundStudent = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Test_AddCourse_AddsCourseToStudent()
    {
        //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Student testStudent = new Student("Brandon", date1);
      testStudent.Save();

      Course testCourse1 = new Course("History", "HIST200");
      testCourse1.Save();

      Course testCourse2 = new Course("Spanish", "SPAN200");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse1);
      testStudent.AddCourse(testCourse2);

      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course>{testCourse1, testCourse2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesStudentFromDatabase()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      string name1 = "Brandon";
      Student testStudent1 = new Student(name1, date1);
      testStudent1.Save();

      string name2 = "Spanish";
      Student testStudent2 = new Student(name2, date1);
      testStudent2.Save();


      //Act
      testStudent1.Delete();
      List<Student> resultStudents = Student.GetAll();
      List<Student> testStudentList = new List<Student> {testStudent2};

      //Assert
      Assert.Equal(testStudentList, resultStudents);
    }


    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }

  }
}
