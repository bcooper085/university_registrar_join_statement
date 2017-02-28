using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/course/form"] = _ => {
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };
      Post["/course/form"] = _ => {
        Course newCourse = new Course(Request.Form["name"], Request.Form["course_number"]);
        newCourse.Save();
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };
      Get["/student/form"] = _ => {
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
      Post["/student/form"] = _ => {
        Student newStudent = new Student(Request.Form["name"], Request.Form["enrollment_date"]);
        newStudent.Save();
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
    }
  }
}
