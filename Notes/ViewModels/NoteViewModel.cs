using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Notes.Models;

namespace Notes.ViewModels;

internal class NoteViewModel : ObservableObject, IQueryAttributable
{
    private Note _note;

    public NoteViewModel()
    {
        _note = new Note();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public NoteViewModel(Note note)
    {
        _note = note;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _note.Date;

    /// <summary>
    /// ItemId is filename of the note my/device/XYZ.notes.txt
    /// </summary>
    public string Identifier => _note.Filename;

    /// <summary>
    /// Commands to be called by the view
    /// </summary>
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    private async Task Save()
    {
        _note.Date = DateTime.Now;
        _note.Save();

         /***
         * A query string parameter is added to the .. navigation path, 
         * indicating which action was taken and the note's unique identifier.
         */
        await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
    }

    private async Task Delete()
    {
        _note.Delete();

        /***
         * A query string parameter is added to the .. navigation path, 
         * indicating which action was taken and the note's unique identifier.
         */
        await Shell.Current.GoToAsync($"..?deleted={_note.Filename}");
    }

    /// <summary>
    /// When a page, or the binding context of a page, implements this interface (IQueryAttributable),
    /// the query string parameters used in navigation are passed to the
    /// ApplyQueryAttributes method. This viewmodel is used as the binding context
    /// for the Note view. When the Note view is navigated to, the view's binding context 
    /// (this viewmodel) is passed the query string parameters used during navigation.
    /// </summary>
    /// <param name="query"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // the value of ?load= should be the identifier (the file name) 
        if (query.ContainsKey("load"))
        {
            _note = Note.Load(query["load"].ToString()!);
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _note = Note.Load(_note.Filename);
        RefreshProperties();
    }

    /// <summary>
    /// The RefreshProperties method is another helper method to ensure
    /// that any subscribers bound to this object are notified that the 
    /// Text and Date properties have changed. Since the underlying model 
    /// (the _note field) is changed when the note is loaded during navigation,
    /// the Text and Date properties aren't actually set to new values.
    /// 
    /// Since these properties aren't directly set, any bindings attached to
    /// those properties wouldn't be notified because OnPropertyChanged isn't
    /// called for each property. RefreshProperties ensures bindings to these 
    /// properties are refreshed.
    /// </summary>
    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}