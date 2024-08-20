using MCVCude.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MCVCude.DataAccess
{
    public class EmployeeDataAccess
    {
        private readonly string? _connectionString;

        public EmployeeDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Employee> GetEmployees()
        {
            var employeeLst = new List<Employee>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from Employee",con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employeeLst.Add(new Employee
                    {
                        Id =Convert.ToInt32(reader["Id"]),
                        Name= reader["Name"].ToString(),
                        Position= reader["Position"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return employeeLst;
        }

        public  void AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO EMPLOYEE (Name,Position, Salary) VALUES(@Name,@Position,@Salary)", con);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                con.Open();
                try 
                {
					cmd.ExecuteNonQuery();
				}
                catch(Exception ex)
                {

                }
                
			}
        }
        public Employee GetEmployeeById(int id)
        {
            Employee employee = new Employee();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("bkey_GetEmpDataById", con);
                cmd.CommandType=CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userId", id)); 
                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    if(reader.Read())
                    {

                        employee.Name = reader["Name"].ToString();
                        employee.Position = reader["Position"] != DBNull.Value ? reader["Position"].ToString() : string.Empty;
                        employee.Salary = Convert.ToDecimal(reader["Salary"]);
                        
                        
                    }
                    //SqlDataReader reader = cmd.ExecuteReader();
                    

                    
                }
                catch (Exception ex) 
                { 

                }
                return employee;

            }

        }
        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateEmpInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserID", employee.Id));
                cmd.Parameters.Add(new SqlParameter("@Name", employee.Name));
                cmd.Parameters.Add(new SqlParameter("@Position", employee.Position));
                cmd.Parameters.Add(new SqlParameter("@Salary", employee.Salary));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
