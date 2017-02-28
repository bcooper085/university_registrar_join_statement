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

    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }

  }
}
