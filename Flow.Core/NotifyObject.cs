using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Flow.Core
{
  public abstract class NotifyObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void Notify([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
  }
}
