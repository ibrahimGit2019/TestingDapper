using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TestingDapper.Assets.classes;
using static Dapper.SqlMapper;

namespace TestingDapper
{
    public static class _Dapper
    {
        public static IDbConnection dbConnection;

        static _Dapper()
        {
            if (dbConnection == null)
            {
                dbConnection = new SqlConnection(Helper.ConnectionString(int.MaxValue));
                dbConnection.Open();
            }
        }


        public static IEnumerable<Users> MapMultipleModels()
        {

            string sql = "select * from Users t1 right join contacts t2 on t1.contactFK=t2.ID";
            var result = dbConnection.Query<Users, Contacts, Users>(sql, (user, contact) => { user._ContactFK = contact; return user; });
            return result;
        }


        public static IEnumerable<Users> MapMultipleModelsWithParameters(string whereValue)
        {

            string sql = $"select * from Users t1 right join contacts t2 on t1.contactFK=t2.ID where t1.lastname like '%'+@lastName+'%'";
            var result = dbConnection.Query<Users, Contacts, Users>(sql, (user, contact) => { user._ContactFK = contact; return user; }
            , new { lastName = whereValue });
            return result;
        }


        public static (List<Users>, List<Contacts>) MultipleSets()
        {

            string sql = $"select * from users;select * from contacts;";
            var result = dbConnection.QueryMultiple(sql);
            List<Users> users = null;
            List<Contacts> contacts = null;
            using (result)
            {
                users = result.Read<Users>().AsList();
                contacts = result.Read<Contacts>().AsList();
            }
            return (users, contacts);
        }


        public static (List<Users>, List<Contacts>) MultipleSetsWithParameters(string firstWhereValue, string secondWhereValue)
        {

            string sql = $"select * from users where firstname=@firstname;select * from contacts where PhoneNumber like '%'+@PhoneNumber+'%';";
            var result = dbConnection.QueryMultiple(sql, new { firstname = firstWhereValue, PhoneNumber = secondWhereValue });
            List<Users> users = null;
            List<Contacts> contacts = null;
            using (result)
            {
                users = result.Read<Users>().AsList();
                contacts = result.Read<Contacts>().AsList();
            }
            return (users, contacts);
        }


        public static long insert(IDbTransaction dbTransaction = null, params string[] input)
        {
            string sql = $"insert into users (firstname,lastname,contactFK) values (@firstname,@lastname,@contactFK);" +
                         $"SELECT @id=SCOPE_IDENTITY();";
            var dp = new DynamicParameters();
            dp.Add("@id", 0, DbType.Int64, ParameterDirection.Output);
            dp.Add("@firstname", input[0]);
            dp.Add("@lastname", input[1]);
            dp.Add("@contactFK", input[2]);

            if (dbTransaction == null)
            {
                dbConnection.Execute(sql, dp);
            }
            else
            {
                dbConnection.Execute(sql, dp, transaction: dbTransaction);
            }

            return dp.Get<long>("@id");
        }


        public static void Transaction()
        {
            using (var tran = dbConnection.BeginTransaction())
            {
                System.Console.WriteLine(insert(tran, input: new string[] { "ismaeel", "Obed", "4" }));

                try
                {
                    //dbConnection.Execute("update users set id=1");  just to make rollback
                    tran.Commit();
                }
                catch (System.Exception e)
                {
                    tran.Rollback();
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public static int InsertBulk()
        {
            return dbConnection.Execute("sp_insertbulkUsers", new { users = getSampleDatatable() }, commandType: CommandType.StoredProcedure);
        }


        public static ICustomQueryParameter getSampleDatatable()
        {
            var dt = new DataTable();
            dt.Columns.Add("firstname", typeof(string));
            dt.Columns.Add("lastname", typeof(string));
            dt.Columns.Add("contactFK", typeof(long));

            dt.Rows.Add(new object[] { "sara", "ali", 5 });
            dt.Rows.Add(new object[] { "Khalid", "sanaydee", 4 });
            dt.Rows.Add(new object[] { "zool", "nasi", 2 });
            dt.Rows.Add(new object[] { "life ", "diffcult", 6 });

            return dt.AsTableValuedParameter();
        }
    }
}
