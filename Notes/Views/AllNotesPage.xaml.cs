namespace Notes.Views;

public partial class AllNotesPage : ContentPage
{
    public AllNotesPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // the CollectionView UI element
        // was given the name in the .xaml
        // <CollectionView x:Name="notesCollection"

        notesCollection.SelectedItem = null;
    }
}