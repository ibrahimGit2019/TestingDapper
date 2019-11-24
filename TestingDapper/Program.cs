using System;
using System.Threading.Tasks;
namespace TestingDapper
{
    class Program
    {
        static void Main(string[] args)
        {

            //foreach (var i in _Dapper.MapMultipleModels())
            //{
            //    Console.WriteLine((i.firstName ?? "None") + " Phone Number :" + i?._ContactFK.PhoneNumber);
            //}

            //foreach (var i in _Dapper.MapMultipleModelsWithParameters("Ob"))
            //{
            //    Console.WriteLine((i.firstName ?? "None") + " Phone Number :" + i?._ContactFK.PhoneNumber);
            //}


            //var (users, contacts) = _Dapper.MultipleSets();

            //foreach (var item in users)
            //{
            //    Console.WriteLine(item.firstName);
            //}
            //foreach (var item in contacts)
            //{
            //    Console.WriteLine(item.PhoneNumber);

            //}


            //var (users, contacts) = _Dapper.MultipleSetsWithParameters("ibrahim","8");

            //foreach (var item in users)
            //{
            //    Console.WriteLine(item.firstName);
            //}
            //foreach (var item in contacts)
            //{
            //    Console.WriteLine(item.PhoneNumber);

            //}
            //Console.WriteLine(_Dapper.insert(input: new string[] { "Jumma", "Dubaa", "3" }));

            //_Dapper.Transaction();

            Console.WriteLine(_Dapper.InsertBulk());

        }

        static void _Parallel()
        {
            int[] dappers = new int[1000];

            Parallel.ForEach(dappers, (dapper) =>
            {
                foreach (var i in _Dapper.MapMultipleModels())
                {
                    Console.WriteLine(i.firstName + " Phone Number :" + i?._ContactFK.PhoneNumber);
                }
            });
        }
    }
}
