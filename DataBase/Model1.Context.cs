﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Auto_Store_Project.DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class user9Entities : DbContext
    {
        private static user9Entities _context;
        public user9Entities()
            : base("name=user9Entities")
        {
        }
        public static user9Entities GetContext()
        {
            if (_context == null)
            {
                _context = new user9Entities();
            }
            return _context;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<PickUpPoints> PickUpPoints { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsInOrder> ProductsInOrder { get; set; }
        public virtual DbSet<StatusOrder> StatusOrder { get; set; }
    }
}
