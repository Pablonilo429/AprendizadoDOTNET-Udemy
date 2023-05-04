﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFilmes.Data;
using MVCFilmes.Models;

namespace MVCFilmes.Controllers
{
    public class FilmesAController : Controller
    {
        private readonly MVCFilmesContext _context;

        public FilmesAController(MVCFilmesContext context)
        {
            _context = context;
        }

        // GET: FilmesA
        public async Task<IActionResult> Index(string Texto, string Genero)
        {
         
           
            //Qquery Genero
            IQueryable<string> generos = from m in _context.Filmes orderby m.Genero select  m.Genero; 

            //Query Filme
            var filmes = from m in _context.Filmes select m;

            //Filtro Genero
            if (!string.IsNullOrEmpty(Genero))
            {
                filmes = filmes.Where(s => s.Genero == Genero); 
            }


            //Filtro Filmes
            if (!string.IsNullOrWhiteSpace(Texto))
            {
                filmes = filmes.Where(s => s.Titulo!.Contains(Texto));
            }

            var filmeViewModel = new Models.FilmesViewModel()
            {
                Filmes = await filmes.ToListAsync(),
                Generos = new SelectList(await generos.Distinct().ToListAsync())
            };

            return _context.Filmes != null ? 
                          View(filmeViewModel) :
                          Problem("Entity set 'MVCFilmesContext.Filmes'  is null.");
        }

        // GET: FilmesA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Filmes == null)
            {
                return NotFound();
            }

            var filmes = await _context.Filmes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmes == null)
            {
                return NotFound();
            }

            return View(filmes);
        }

        // GET: FilmesA/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FilmesA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,DataLancamento,Genero,Preco,Pontuacao")] Filmes filmes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filmes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filmes);
        }

        // GET: FilmesA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Filmes == null)
            {
                return NotFound();
            }

            var filmes = await _context.Filmes.FindAsync(id);
            if (filmes == null)
            {
                return NotFound();
            }
            return View(filmes);
        }

        // POST: FilmesA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,DataLancamento,Genero,Preco,Pontuacao")] Filmes filmes)
        {
            if (id != filmes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmesExists(filmes.Id))
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
            return View(filmes);
        }

        // GET: FilmesA/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Filmes == null)
            {
                return NotFound();
            }

            var filmes = await _context.Filmes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmes == null)
            {
                return NotFound();
            }

            return View(filmes);
        }

        // POST: FilmesA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Filmes == null)
            {
                return Problem("Entity set 'MVCFilmesContext.Filmes'  is null.");
            }
            var filmes = await _context.Filmes.FindAsync(id);
            if (filmes != null)
            {
                _context.Filmes.Remove(filmes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmesExists(int id)
        {
          return (_context.Filmes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
