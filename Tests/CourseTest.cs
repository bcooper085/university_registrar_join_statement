using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CourseDatabaseEmptyOnLoad_Empty()
    {
      int result = Course.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save()
    {
      Course testCourse = new Course("History", "HIST200");
      testCourse.Save();

      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Course testCourse = new Course("History", "HIST200");
      testCourse.Save();

      //Act
      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindsCourseInDatabase()
    {
      //Arrange

      Course testCourse = new Course("History", "HIST200");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, foundCourse);
    }

    [Fact]
    public void Test_AddStudent_AddsStudentToCourse()
    {
        //Arrange
      Course testCourse = new Course("History", "HIST200");
      testCourse.Save();

      DateTime date1 = new DateTime(2008, 4, 10);
      string name1 = "Brandon";
      Student testStudent1 = new Student(name1, date1);
      testStudent1.Save();

      DateTime date2 = new DateTime(2016, 4, 10);
      string name2 = "Joe";
      Student testStudent2 = new Student(name2, date2);
      testStudent2.Save();

      //Act
      testCourse.AddStudent(testStudent1);
      testCourse.AddStudent(testStudent2);

      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student>{testStudent1, testStudent2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesCourseFromDatabase()
    {
      //Arrange
      string name1 = "History";
      Course testCourse1 = new Course(name1, "HIST200");
      testCourse1.Save();

      string name2 = "Spanish";
      Course testCourse2 = new Course(name2, "SPAN200");
      testCourse2.Save();


      //Act
      testCourse1.Delete();
      List<Course> resultCourses = Course.GetAll();
      List<Course> testCourseList = new List<Course> {testCourse2};

      //Assert
      Assert.Equal(testCourseList, resultCourses);
    }

    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }
  }
}
