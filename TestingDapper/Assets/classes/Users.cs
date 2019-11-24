using System;

namespace TestingDapper.Assets.classes
{
    public class Users : Root
    {
        public DateTime createdDate;
        public long id;
        public Contacts _ContactFK;
        public string firstName;
        public string lastName;
    }
}
