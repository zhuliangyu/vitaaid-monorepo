using System.Windows.Controls.Primitives;

namespace MyToolkit.Base
{
    public interface IWizTransitioner
    {
        Selector TransitionerObj { get; }
        string Title { get; }
        void PageEntered(int iStep);
        bool PageNexting(int iStep);
    }
}
