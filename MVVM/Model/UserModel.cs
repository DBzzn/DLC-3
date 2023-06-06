using System.Linq;
using System.Collections.ObjectModel;


namespace DLC_3.MVVM.Model
{
    class UserModel
    {
        public string Username { get; set; }
        public string UID { get; set; }
        public string ImageSource { get; set; }
        public ObservableCollection<MessageModel> Messages{ get; set; }
        public string LastMessage => Messages.Last().Message;


    }
}
