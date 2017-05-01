using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web;
using System.Web.Script.Services;
using System.Globalization;
using HtmlAgilityPack;
using System.Text;
using System.Linq;


namespace WebApplication3
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class EmployeeService : System.Web.Services.WebService
    {
        [WebMethod]
        public void GetAllEmployees()
        {
            List<Employee> ListEmployees = new List<Employee>();
            string cs = ConfigurationManager.ConnectionStrings["TestEmplyee"].ConnectionString;
            using(SqlConnection con  = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr =  cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Employee employee = new Employee();
                    employee.EmpId = Convert.ToInt32(rdr["EmpId"]);
                    employee.EmpName = rdr["EmpName"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Salary = Convert.ToDecimal(rdr["Salary"].ToString());
                    employee.DeptId = Convert.ToInt32(rdr["DeptId"].ToString());

                    ListEmployees.Add(employee);
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Write(js.Serialize(ListEmployees));
            }
        }

        [WebMethod]
        public void GetAllEmployeesYearCount()
        {
            List<Employee> ListEmployees = new List<Employee>();
            string cs = ConfigurationManager.ConnectionStrings["TestEmplyee"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Employee employee = new Employee();
                    employee.EmpId = Convert.ToInt32(rdr["EmpId"]);
                    employee.EmpName = rdr["EmpName"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Salary = Convert.ToDecimal(rdr["Salary"].ToString());
                    employee.DeptId = Convert.ToInt32(rdr["DeptId"].ToString());

                    ListEmployees.Add(employee);
                }
                var empGp = (from p in ListEmployees
                            group p by p.Gender into g
                            select new EmployeeYearCount { Gender = g.Key, Count = g.ToList().Count }).ToList();
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Write(js.Serialize(empGp));
            }
        }

        [WebMethod]
        public void GetAllDepartment()
        {
            List<Department> ListDepartment = new List<Department>();
            string cs = ConfigurationManager.ConnectionStrings["TestEmplyee"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Department department = new Department();
                    
                    department.DeptId = Convert.ToInt32(rdr["DeptId"]);
                    department.DeptName = rdr["DeptName"].ToString();
                    department.DeptHead = rdr["DeptHead"].ToString();
                    department.DeptLocation = rdr["DeptLocation"].ToString();
                    
                    ListDepartment.Add(department);
                }
                JavaScriptSerializer js = new JavaScriptSerializer();

                Context.Response.Write(js.Serialize(ListDepartment));
            }
        }

        [WebMethod]
        [ScriptMethod]
        public void GetAllEmployeeByDate(string date)
        {
            var dtStr = date.Substring(1, date.Length - 2);
            CultureInfo provider = CultureInfo.InvariantCulture;
            var dt = DateTime.ParseExact(dtStr,"yyyy/MM/dd",provider);
            List<Employee> ListEmployees = new List<Employee>();
            string cs = ConfigurationManager.ConnectionStrings["TestEmplyee"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployeeByDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param1", dt);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Employee employee = new Employee();
                    employee.EmpId = Convert.ToInt32(rdr["EmpId"]);
                    employee.EmpName = rdr["EmpName"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Salary = Convert.ToDecimal(rdr["Salary"].ToString());
                    employee.DeptId = Convert.ToInt32(rdr["DeptId"].ToString());

                    ListEmployees.Add(employee);
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                var k = js.Serialize(ListEmployees);
                Context.Response.ContentType = "application/json; charset=utf-8";
                Context.Response.Flush();
               
                
                //Context.Response.Clear();
                
                //var js = 
                //var js = Json.JsonParser.Serialize<List<Employee>>(ListEmployees);
                Context.Response.Write(k);
                Context.Response.End();
                //Context.Response.Flush();
            }
        }
    }
}
