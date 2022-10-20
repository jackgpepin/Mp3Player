using Mp3Player.Models;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private string _name;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _id;

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    private Profile _profile;
    
    public ProfileViewModel(Profile profile)
    {
        _profile = profile;
        Name = profile.Name;
        Id = profile.Id;
    }

    public void Delete()
    {
        _profile.Delete();
    }
}