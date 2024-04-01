
namespace Rpo.Identity.Model.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class EmployeeAuthentication
    {
        public class EmployeeDetails
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public int IdGroup { get; set; }

            public bool IsActive { get; set; }

            public bool IsArchive { get; set; }

        }

        /// <summary>
        /// Gets the employee by identifier.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <returns>EmployeeDetails.</returns>
        public EmployeeDetails GetEmployeeById(int employeeId)
        {
            string connetionString = null;
            SqlConnection cnn;
#if DEBUG
            connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["RpoConnection"].ConnectionString;
#else
            connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["RpoProdConnection"].ConnectionString;
#endif
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Employees] WHERE Id=@Id", cnn);
                cmd.Parameters.Add(new SqlParameter("@Id", employeeId));
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow employeeDr = dt.Rows[0];
                    EmployeeDetails employeeDetails = new EmployeeDetails();
                    employeeDetails.Id = Convert.ToInt32(employeeDr["Id"]);
                    employeeDetails.FirstName = Convert.ToString(employeeDr["FirstName"]);
                    employeeDetails.LastName = Convert.ToString(employeeDr["LastName"]);
                    employeeDetails.Email = Convert.ToString(employeeDr["Email"]);
                    employeeDetails.IdGroup = Convert.ToInt32(employeeDr["IdGroup"]);
                    employeeDetails.IsActive = Convert.ToBoolean(employeeDr["IsActive"]);
                    employeeDetails.IsArchive = Convert.ToBoolean(employeeDr["IsArchive"]);

                    return employeeDetails;
                }
                else
                {
                    return new EmployeeDetails();
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                return new EmployeeDetails();
            }
        }

        /// <summary>
        /// Gets the employee by email.
        /// </summary>
        /// <param name="employeeByEmail">The employee by email.</param>
        /// <returns>EmployeeDetails.</returns>
        public EmployeeDetails GetEmployeeByEmail(string employeeByEmail)
        {
            string connetionString = null;
            SqlConnection cnn;
#if DEBUG
            connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["RpoConnection"].ConnectionString;
#else
            connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["RpoProdConnection"].ConnectionString;
#endif
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Employees] WHERE Email=@Email", cnn);
                cmd.Parameters.Add(new SqlParameter("@Email", employeeByEmail));
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow employeeDr = dt.Rows[0];
                    EmployeeDetails employeeDetails = new EmployeeDetails();
                    employeeDetails.Id = Convert.ToInt32(employeeDr["Id"]);
                    employeeDetails.FirstName = Convert.ToString(employeeDr["FirstName"]);
                    employeeDetails.LastName = Convert.ToString(employeeDr["LastName"]);
                    employeeDetails.Email = Convert.ToString(employeeDr["Email"]);
                    employeeDetails.IdGroup = Convert.ToInt32(employeeDr["IdGroup"]);
                    employeeDetails.IsActive = Convert.ToBoolean(employeeDr["IsActive"]);
                    employeeDetails.IsArchive = Convert.ToBoolean(employeeDr["IsArchive"]);

                    return employeeDetails;
                }
                else
                {
                    return new EmployeeDetails();
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                return new EmployeeDetails();
            }
        }


        /// <summary>
        /// Gets the employee by email.
        /// </summary>
        /// <param name="employeeByEmail">The employee by email.</param>
        /// <returns>EmployeeDetails.</returns>
        public EmployeeDetails GetCustomerByEmail(string employeeByEmail)
        {
            string connetionString = null;
            SqlConnection cnn;
#if DEBUG
            connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["RpoConnection"].ConnectionString;
#else
            connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["RpoProdConnection"].ConnectionString;
#endif
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Customers] WHERE EmailAddress=@Email", cnn);
                cmd.Parameters.Add(new SqlParameter("@Email", employeeByEmail));
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow employeeDr = dt.Rows[0];
                    EmployeeDetails employeeDetails = new EmployeeDetails();
                    employeeDetails.Id = Convert.ToInt32(employeeDr["Id"]);
                    employeeDetails.FirstName = Convert.ToString(employeeDr["FirstName"]);
                    employeeDetails.LastName = Convert.ToString(employeeDr["LastName"]);
                    employeeDetails.Email = Convert.ToString(employeeDr["EmailAddress"]);
                    employeeDetails.IdGroup = Convert.ToInt32(employeeDr["IdGroup"]);
                    employeeDetails.IsActive = Convert.ToBoolean(employeeDr["IsActive"]);
                    // employeeDetails.IsArchive = Convert.ToBoolean(employeeDr["IsArchive"]);
                    employeeDetails.IsArchive = false;
                    cnn.Close();
                    return employeeDetails;

                }
                else
                {
                    cnn.Close();
                    return new EmployeeDetails();
                }

            }
            catch (Exception ex)
            {
                return new EmployeeDetails();
            }
        }
    }
}
