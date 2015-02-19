using System.Collections.Generic;

namespace Architecture.ViewModel
{
    public class FindByLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}