﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleDbModelEntities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SimpleDbEntities : DbContext
    {
        public SimpleDbEntities()
            : base("name=SimpleDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SimpleTable> SimpleTables { get; set; }
    
        public virtual int SimpleInsertSproc(string param1, Nullable<int> param2)
        {
            var param1Parameter = param1 != null ?
                new ObjectParameter("param1", param1) :
                new ObjectParameter("param1", typeof(string));
    
            var param2Parameter = param2.HasValue ?
                new ObjectParameter("param2", param2) :
                new ObjectParameter("param2", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SimpleInsertSproc", param1Parameter, param2Parameter);
        }
    }
}
