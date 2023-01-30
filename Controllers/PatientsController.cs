using AnarkanHospital.Context;
using AnarkanHospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnarkanHospital.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string? search, string sortType, string sortOrder, int pageSize = 2, int pageNumber = 1)
        {
            IQueryable<Patient> pat = _context.Patients.AsQueryable();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                search = search.Trim();
                pat = _context.Patients.Where(d => d.Name.Contains(search));
                ViewBag.CurrentSearch = search;
            }

            if (!string.IsNullOrWhiteSpace(sortType) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                if (sortType == "Name")
                {
                    if (sortOrder == "asc")
                    {
                        pat = _context.Patients.OrderBy(d => d.Name);
                    }
                    else if (sortOrder == "desc")
                    {
                        pat = _context.Patients.OrderByDescending(d => d.Name);
                    }
                }

                else if (sortType == "HealthProblem")
                {
                    if (sortOrder == "asc")
                    {
                        pat = _context.Patients.OrderBy(d => d.HealthProblem);
                    }
                    else if (sortOrder == "desc")
                    {
                        pat = _context.Patients.OrderByDescending(d => d.HealthProblem);
                    }
                }


            }

            pat = pat.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.CurrentSearch = search;

            return View(nameof(Index), pat);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            Patient pat = await _context.Patients.Include(p => p.Doctor).FirstOrDefaultAsync(p => p.Id == id);

            if (pat == null)
            {
                return NotFound();
            }


            return View(pat);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {

            ViewBag.AllDoctors = _context.Doctors.ToList();
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,HealthProblem,Critical,ArrivedAt,LeavingAt,CreatedAt,LastUpdatedAt,DoctorId")] Patient patient)
        {
            if (ModelState.IsValid)
            {

                patient.CreatedAt = DateTime.Now;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                ViewBag.AllDoctors = await _context.Doctors.ToListAsync();

                return View(nameof(Create), patient);
            }

        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.AllDoctors = _context.Doctors.ToList();
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,HealthProblem,Critical,ArrivedAt,LeavingAt,CreatedAt,LastUpdatedAt,DoctorId")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                return View(nameof(Edit));
            }

        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Patient pat = await _context.Patients.FirstOrDefaultAsync(m => m.Id == id);

            _context.Patients.Remove(pat);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
