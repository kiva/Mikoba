using System.Threading.Tasks;

namespace mikoba.ViewModels
{
    public interface IBaseViewModel
    {
        string Name { get; set; }

        string Title { get; set; }

        bool IsBusy { get; set; }

        Task InitializeAsync(object navigationData);
    }
}
