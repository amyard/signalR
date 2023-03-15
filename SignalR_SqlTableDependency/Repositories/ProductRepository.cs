using System.Data;
using System.Data.SqlClient;
using SignalR_SqlTableDependency.Models;

namespace SignalR_SqlTableDependency.Repositories;

public class ProductRepository
{
    private string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Product> GetProducts()
    {
        List<Product> products = new();

        var data = GetProductDetailsFromDb();
        foreach (DataRow row in data.Rows)
        {
            Product product = new Product()
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Category = row["Category"].ToString(),
                Price = Convert.ToDecimal(row["Price"])
            };
            
            products.Add(product);
        }

        return products;
    }

    public DataTable GetProductDetailsFromDb()
    {
        var query = "SELECT Id, Name, Category, Price FROM Product";
        DataTable dataTable = new DataTable();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

    }
}