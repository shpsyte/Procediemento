﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace b2yweb_mvc4.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class b2yweb_entities : DbContext
    {
        public b2yweb_entities() : base("name=b2yweb_entities")
        {
        }

        public b2yweb_entities(String strEntity)
            : base("name=" + strEntity + "_entities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<GUsuario> GUsuario { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
