using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Reflection.Emit;

namespace PgTestTool
{
    class Program
    {
        static void Main(string[] args)
        {
        start:
            Console.WriteLine("请输入目标数据库IP：【默认：127.0.0.1】");
            var host = Console.ReadLine();
            host = string.IsNullOrWhiteSpace(host) ? "127.0.0.1" : host;

            Console.WriteLine("请输入目标数据库端口号：【默认：5432】");
            var port = Console.ReadLine();
            port = string.IsNullOrWhiteSpace(port) ? "5432" : port;

            Console.WriteLine("请输入目标数据库名称：【默认：postgres】");
            var database = Console.ReadLine();
            database = string.IsNullOrWhiteSpace(database) ? "postgres" : database;

            Console.WriteLine("请输入目标数据库用户名：【默认：postgres】");
            var username = Console.ReadLine();
            username = string.IsNullOrWhiteSpace(username) ? "postgres" : username;

            Console.WriteLine("请输入目标数据库密码：");
            var password = Console.ReadLine();
            //password = string.IsNullOrWhiteSpace(password) ? "" : password;

            var pg = new PgContext(host, port, database, username, password);

            try
            {
                if (pg.Database.CanConnect())
                {
                    Console.WriteLine("连接成功。");
                }
                else
                {
                    Console.WriteLine("连接失败。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接异常");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                pg?.Dispose();
            }

            Console.WriteLine("是否重新连接？按【y】确认；按【n或其它】退出！");
            if (Console.ReadLine().ToLower() == "y")
            {
                goto start;
            }

            Environment.Exit(0);
        }
    }

    class PgContext : DbContext
    {
        private string host;
        private string port;
        private string database;
        private string username;
        private string password;

        public PgContext(string host, string port, string database, string username, string password)
        {
            this.host = host;
            this.port = port;
            this.database = database;
            this.username = username;
            this.password = password;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={host};Port={port};Database={database};Username={username};Password={password}");
        }
    }
}
