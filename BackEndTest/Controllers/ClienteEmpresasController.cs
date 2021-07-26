using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackEndTest.Data;
using BackEndTest.Domain.Entity;

namespace BackEndTest.Controllers
{
    public class ClienteEmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteEmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }
        // POST

        public async Task<IActionResult>IndexFiltro(string searchString)
        {
            var clientes = from c in _context.Clientes select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(s => s.CPF.Contains(searchString));


            }
            return View(await clientes.ToListAsync());

        }
        // GET: ClienteEmpresas
        public async Task<IActionResult> Index()
        {
           
           
            var applicationDbContext = _context.ClienteEmpresas.Include(c => c.Cliente).Include(c => c.Empresa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClienteEmpresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteEmpresa = await _context.ClienteEmpresas
                .Include(c => c.Cliente)
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clienteEmpresa == null)
            {
                return NotFound();
            }

            return View(clienteEmpresa);
        }

        // GET: ClienteEmpresas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "RazaoSocial");
            return View();
        }

        // POST: ClienteEmpresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,EmpresaId")] ClienteEmpresa clienteEmpresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clienteEmpresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", clienteEmpresa.ClienteId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", clienteEmpresa.EmpresaId);
            return View(clienteEmpresa);
        }

        // GET: ClienteEmpresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteEmpresa = await _context.ClienteEmpresas.FindAsync(id);
            if (clienteEmpresa == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", clienteEmpresa.ClienteId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", clienteEmpresa.EmpresaId);
            return View(clienteEmpresa);
        }

        // POST: ClienteEmpresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,EmpresaId")] ClienteEmpresa clienteEmpresa)
        {
            if (id != clienteEmpresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clienteEmpresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteEmpresaExists(clienteEmpresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", clienteEmpresa.ClienteId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", clienteEmpresa.EmpresaId);
            return View(clienteEmpresa);
        }

        // GET: ClienteEmpresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteEmpresa = await _context.ClienteEmpresas
                .Include(c => c.Cliente)
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clienteEmpresa == null)
            {
                return NotFound();
            }

            return View(clienteEmpresa);
        }

        // POST: ClienteEmpresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clienteEmpresa = await _context.ClienteEmpresas.FindAsync(id);
            _context.ClienteEmpresas.Remove(clienteEmpresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteEmpresaExists(int id)
        {
            return _context.ClienteEmpresas.Any(e => e.Id == id);
        }
    }
}
