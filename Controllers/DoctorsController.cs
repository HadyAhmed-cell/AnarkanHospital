using AnarkanHospital.Context;
using AnarkanHospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnarkanHospital.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DoctorsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string? search, string sortType, string sortOrder, int pageSize = 2, int pageNumber = 1)
        {
            IQueryable<Doctor> docs = _context.Doctors.AsQueryable();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                search = search.Trim();
                docs = _context.Doctors.Where(d => d.Name.Contains(search));
                ViewBag.CurrentSearch = search;
            }

            if (!string.IsNullOrWhiteSpace(sortType) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                if (sortType == "Name")
                {
                    if (sortOrder == "asc")
                    {
                        docs = _context.Doctors.OrderBy(d => d.Name);
                    }
                    else if (sortOrder == "desc")
                    {
                        docs = _context.Doctors.OrderByDescending(d => d.Name);
                    }
                }

                else if (sortType == "Speciality")
                {
                    if (sortOrder == "asc")
                    {
                        docs = _context.Doctors.OrderBy(d => d.Speciality);
                    }
                    else if (sortOrder == "desc")
                    {
                        docs = _context.Doctors.OrderByDescending(d => d.Speciality);
                    }
                }

                else if (sortType == "Salary")
                {
                    if (sortOrder == "asc")
                    {
                        docs = _context.Doctors.OrderBy(d => d.Salary);
                    }
                    else if (sortOrder == "desc")
                    {
                        docs = _context.Doctors.OrderByDescending(d => d.Salary);
                    }
                }
            }

            docs = docs.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.CurrentSearch = search;

            return View(nameof(Index), docs);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            Doctor pat = await _context.Doctors.Include(p => p.Patient).FirstOrDefaultAsync(p => p.Id == id);


            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Speciality,Age,Email,Password,PhoneNumber,Availability,HireDate,Salary,HiringDateTime,BirthDate,AttendanceTime,LeavingTime,CreatedAt,LastUpdatedAt,ConfirmPassword,ConfirmEmail")] Doctor doctor, IFormFile? imageFile, IFormFile? cvFile)
        {




            if (ModelState.IsValid)
            {

                if (imageFile == null)
                {
                    doctor.ImgUrl = "\\images\\No_Image.png";
                }

                if (cvFile == null)
                {
                    doctor.CvUrl = "\\cvs\\No_CV.pdf";
                }
                if (cvFile != null)
                {
                    string cvExtension = Path.GetExtension(cvFile.FileName);
                    Guid cvGuid = Guid.NewGuid();
                    string cvName = cvGuid + cvExtension;
                    string cvUrl = "\\cvs\\" + cvName;
                    doctor.CvUrl = cvUrl;

                    string cvPath = _environment.WebRootPath + cvUrl;

                    FileStream cvStream = new FileStream(cvPath, FileMode.Create);
                    cvFile.CopyTo(cvStream);
                    cvStream.Dispose();

                }



                if (imageFile != null)
                {
                    string imgExtention = Path.GetExtension(imageFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtention;
                    string imgUrl = "\\images\\" + imgName;
                    doctor.ImgUrl = imgUrl;

                    string imgPath = _environment.WebRootPath + imgUrl;

                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();

                }





                doctor.CreatedAt = DateTime.Now;
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Speciality,Age,Email,Password,PhoneNumber,Availability,Salary,HiringDateTime,BirthDate,AttendanceTime,LeavingTime,CreatedAt,LastUpdatedAt,ConfirmPassword,ConfirmEmail,ImgUrl,CvUrl")] Doctor doctor, IFormFile imageFile, IFormFile cvFile)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            else
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null)
                    {
                        if (doctor.ImgUrl != "\\images\\No_Image.png")
                        {
                            string oldImagePath = _environment.WebRootPath + doctor.ImgUrl;

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }



                        }

                        string imgExtension = Path.GetExtension(imageFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgUrl = "\\images\\" + imgName;
                        doctor.ImgUrl = imgUrl;

                        string imgPath = _environment.WebRootPath + imgUrl;

                        FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                        imageFile.CopyTo(imgStream);
                        imgStream.Dispose();
                    }

                    if (cvFile != null)
                    {
                        if (doctor.CvUrl != "\\cvs\\No_CV.pdf")
                        {
                            string oldCvPath = _environment.WebRootPath + doctor.CvUrl;

                            if (System.IO.File.Exists(oldCvPath))
                            {
                                System.IO.File.Delete(oldCvPath);
                            }



                        }

                        string cvExtension = Path.GetExtension(cvFile.FileName);
                        Guid cvGuid = Guid.NewGuid();
                        string cvName = cvGuid + cvExtension;
                        string cvUrl = "\\cvs\\" + cvName;
                        doctor.CvUrl = cvUrl;

                        string cvPath = _environment.WebRootPath + cvUrl;

                        FileStream cvStream = new FileStream(cvPath, FileMode.Create);
                        cvFile.CopyTo(cvStream);
                        cvStream.Dispose();
                    }
                    doctor.LastUpdatedAt = DateTime.Now;
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));

                }
            }


            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Doctors'  is null.");
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteCurrent(int id)
        {
            Doctor doc = _context.Doctors.FirstOrDefault(e => e.Id == id);

            if (doc.ImgUrl != "\\images\\No_Image.png")
            {
                string imgPath = _environment.WebRootPath + doc.ImgUrl;

                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }

            if (doc.CvUrl != "\\cvs\\No_CV.pdf")
            {
                string cvPath = _environment.WebRootPath + doc.CvUrl;

                if (System.IO.File.Exists(cvPath))
                {
                    System.IO.File.Delete(cvPath);
                }
            }

            _context.Doctors.Remove(doc);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
