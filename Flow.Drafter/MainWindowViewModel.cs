using Flow.Core.Model;
using Flow.Core.ViewModel;
using System.Collections.ObjectModel;

namespace Flow.Drafter
{
  internal sealed class MainWindowViewModel
  {
    public MainWindowViewModel()
    {
      Nodes = new ObservableCollection<NodeViewModel>()
      {
        new NodeViewModel(new Node() { Position = new() { X = 12, Y = 32 } })
      };
    }

    public ObservableCollection<NodeViewModel> Nodes { get; }
  }
}
