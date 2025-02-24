using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HealthInsuranceClaim.Controllers
{
    public class ClaimController : Controller
    {

        private readonly string pdfDirectory = @"C:\Users\Dhivya S\AppData\Local\Temp\Task";
        private readonly string basicPlanFile = "BasicPlan.pdf";
        private readonly string premiumPlanFile = "PremiumPlan.pdf";
        // Sample patient data
        private static List<Patient> patients = new List<Patient>
        {
            new Patient { PolicyNumber = "123456", Name = "John Doe", AadharNumber = "123412341234", PanNumber = "ABCDE1234F", PolicyType ="Basic" },
            new Patient { PolicyNumber = "654321", Name = "Jane Smith", AadharNumber = "567856785678", PanNumber = "XYZAB5678G", PolicyType ="Premium" },
            new Patient { PolicyNumber = "OS1693844064", Name = "S Dhivya", AadharNumber = "567856785679", PanNumber = "CPNPD4913F", PolicyType ="Basic" },
            new Patient { PolicyNumber = "OS9783843907", Name = "Neha Mishra", AadharNumber = "567856785680", PanNumber = "EUFPM5640K", PolicyType ="Premium"  },
            new Patient { PolicyNumber = "OS9563844005", Name = "Mohd Danish", AadharNumber = "567856785681", PanNumber = "CWNPD0158B", PolicyType ="Premium"  },
            new Patient { PolicyNumber = "OS2373844017", Name = "Shivam Maurya", AadharNumber = "567856785682", PanNumber = "FSNPN31695", PolicyType ="Premium"  },
            new Patient { PolicyNumber = "OS4413843965", Name = "Thasneem Fharuk", AadharNumber = "567856785683", PanNumber = "BUDPT7246P", PolicyType ="Basic" },
        };

        public IActionResult Index()
        {
            return View(patients);
        }
        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return Json(new { success = true, patients });
            }

            var filteredPatients = patients.Where(p =>
                p.PolicyNumber.Contains(searchQuery) ||
                p.AadharNumber.Contains(searchQuery) ||
                p.PanNumber.Contains(searchQuery)
            ).ToList();

            if (!filteredPatients.Any())
            {
                return Json(new { success = false, message = "Policy, Aadhar Number, or PAN Number does not exist!" });
            }

            return Json(new { success = true, patients = filteredPatients });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DownloadPdf(string policyType)
        {
            string fileName = policyType?.ToLower() == "premium" ? premiumPlanFile : basicPlanFile; // Determine file

            var filePath = Path.Combine(pdfDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }
    }
    public class Patient
    {
        public string PolicyNumber { get; set; }
        public string Name { get; set; }
        public string AadharNumber { get; set; }
        public string PanNumber { get; set; }
        public string PolicyType { get; set; }
        public string PolicyPDF { get; set; }
    }
}
