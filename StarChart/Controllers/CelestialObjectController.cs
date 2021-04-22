using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var co = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (co == null) return NotFound();
            co.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == co.Id).ToList();
            return Ok(co);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var objects = _context.CelestialObjects.Where(c => c.Name == name);
            if (!objects.Any()) return NotFound();
            foreach (var celestialObject in objects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(objects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in objects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(objects);
        }
    }
}
