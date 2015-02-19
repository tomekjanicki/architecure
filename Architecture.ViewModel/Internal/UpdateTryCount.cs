using System.ComponentModel.DataAnnotations;

namespace Architecture.ViewModel.Internal
{
    public class UpdateTryCount
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int TryCount { get; set; }
    }
}