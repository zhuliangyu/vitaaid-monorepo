using System.ComponentModel;
using System.Threading.Tasks;

namespace MyToolkit.Base
{
    public interface IWizardPage : INotifyPropertyChanged
    {
        string Title { get; set; }

        string BreadcrumbTitle { get; set; }

        string Description { get; set; }

        int Number { get; set; }
        bool IsOptional { get; }

        Task CancelAsync();
        Task SaveAsync();
    }
}
