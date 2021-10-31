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
        public DpsEnrollmentListViewModel dpsEnrollmentList { get; set; }
        public DpsEnrollmentListViewModel dpsGroupEnrollmentList { get; set; }

        public EiFirmwareListViewModel eiFirmwareList { get; set; }
        public string eiApiKey { get; set; }
    }

    public class IoTHubDeviceListViewModel
    {
        public IoTHubDeviceListViewModel()
        {
            Devices = new List<IoTHubDeviceViewModel>();
        }

        [Display(Name = "IoT Hub Device List")]
        [Required]
        public string SelectedIoTHubDevice { get; set; }
        public IList<IoTHubDeviceViewModel> Devices { get; set; }
    }

    public class IoTHubDeviceViewModel
    {
        public string DeviceId { get; set; }
        public string AuthenticationType {get;set;}
        public bool IsEdge {get;set;}
        public string ModelId {get;set;}
        public string Status {get;set;}
    }

    public class DpsEnrollmentListViewModel
    {
        public DpsEnrollmentListViewModel()
        {
            Enrollments = new List<EnrollmentViewModel>();
        }

        [Display(Name = "DPS Enrollment List")]
        [Required]
        public string SelectedEnrollment { get; set; }
        public IList<EnrollmentViewModel> Enrollments { get; set; }
    }

    public class DpsGroupEnrollmentListViewModel
    {
        public DpsGroupEnrollmentListViewModel()
        {
            Enrollments = new List<EnrollmentViewModel>();
        }

        [Display(Name = "DPS Group Enrollment List")]
        [Required]
        public string SelectedEnrollment { get; set; }
        public IList<EnrollmentViewModel> Enrollments { get; set; }
    }

    public class EnrollmentViewModel
    {
        public string RegistrationId { get; set; }
        public bool isGroup { get; set; }
    }

    // Edge Impulse
    public class EiFirmwareListViewModel
    {
        public EiFirmwareListViewModel()
        {
            Options = new List<FirmwareOptionViewModel>();
        }

        [Display(Name = "EI firmware List")]
        [Required]
        public string SelectedOption { get; set; }
        public IList<FirmwareOptionViewModel> Options { get; set; }
    }

    public class FirmwareOptionViewModel
    {
        public string OptionName { get; set; }
        public string OptionKey { get; set; }
    }
}
