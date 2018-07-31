using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SammysAuto.Models;

namespace SammysAuto.ViewModels
{
    public class CarAndServicesViewModel
    {
        public Car carObj { get; set; }
        public Service NewServiceObj { get; set; }
        public IEnumerable<Service> PastSericesObj { get; set; }
        public List<ServiceType> ServiceTypesObj { get; set; }
    }
}
