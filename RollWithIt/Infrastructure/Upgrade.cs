using System;

namespace RollWithIt.Infrastructure
{
    public class Upgrade
    {
        public Upgrade(Guid id, string name, int order, string sql)
        {
            Id = id;
            Name = name;
            Order = order;
            Sql = sql;
        }
        public Upgrade(string id, string name, int order, string sql) : this(Guid.Parse(id), name, order, sql)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Sql { get; set; }
    }
}
