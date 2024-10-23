using AzureSqlConnectionDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Net.Sockets; // For TCP check
using System.Threading.Tasks;

namespace AzureSqlConnectionDemo.Controllers
{
    public class SqlController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new SqlConnectionModel());
        }

        [HttpPost]
        public async Task<IActionResult> Connect(SqlConnectionModel model)
        {
            if (string.IsNullOrEmpty(model.ConnectionString))
            {
                model.Message = "Connection string cannot be empty.";
                return View("Index", model);
            }

            // SQL Connection Test
            try
            {
                using (SqlConnection connection = new SqlConnection(model.ConnectionString))
                {
                    connection.Open();
                    model.Message = "Connection to Azure SQL Database was successful!";
                }
            }
            catch (Exception ex)
            {
                model.Message = $"SQL Connection Error: {ex.Message}";
            }

            // TCP Ping Test (Server IP/hostname test)
            if (!string.IsNullOrEmpty(model.ServerAddress))
            {
                model.PingTestMessage = await TestTcpConnectionAsync(model.ServerAddress, 1433);
            }
            else
            {
                model.PingTestMessage = "Server address cannot be empty for ping test.";
            }

            return View("Index", model);
        }

        private async Task<string> TestTcpConnectionAsync(string serverAddress, int port)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(serverAddress, port);
                    return $"TCP connection to {serverAddress} on port {port} was successful!";
                }
            }
            catch (SocketException ex)
            {
                return $"TCP connection failed: {ex.Message}";
            }
        }
    }
}
