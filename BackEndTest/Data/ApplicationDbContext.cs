using BackEndTest.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BackEndTest.Domain.ViewModel;

namespace BackEndTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<ClienteEmpresa> ClienteEmpresas { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);







        }

        public DbSet<BackEndTest.Domain.ViewModel.ClienteViewModel> ClienteViewModel { get; set; }

        public DbSet<BackEndTest.Domain.ViewModel.EmpresaViewModel> EmpresaViewModel { get; set; }

        public DbSet<BackEndTest.Domain.ViewModel.ClienteEmpresaViewModel> ClienteEmpresaViewModel { get; set; }
    }
}