using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenPlatform_WebPortal.Models
{
    public class HomeView
    {
        [DisplayName("Select Device")]
        public string deviceId { get; set; }
        public string moduleId { get; set; }
        public IEnumerable<SelectListItem> deviceList { get; set; }
        public IEnumerable<SelectListItem> moduleList { get; set; }
        public string newDeviceId { get; set; }
        public string registrationId { get; set; }
        public IEnumerable<SelectListItem> enrollmentList { get; set; }
        public EnrollmentListSelectorViewModel enrollmentList2 { get; set; }
        public IEnumerable<SelectListItem> groupEnrollmentList { get; set; }
    }

    public class EnrollmentListSelectorViewModel
    {
        public EnrollmentListSelectorViewModel()
        {
            EnrollmentList = new List<EnrollmentListViewModel>();
        }

        [Display(Name = "DPS Enrollment List")]
        [Required]
        public string SelectedEnrollment { get; set; }
        public IList<EnrollmentListViewModel> EnrollmentList { get; set; }
    }

    public class EnrollmentListViewModel
    {
        public string RegistrationId { get; set; }
        public bool isGroup { get; set; }
    }
}
