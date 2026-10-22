using ByteCollector.Commands;

namespace ByteCollector.Events
{
    public class PopupData
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public ICommand ConfirmCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public PopupData(string title, string message, ICommand confirmCommand, ICommand cancelCommand = null)
        {
            Title = title;
            Message = message;
            ConfirmCommand = confirmCommand;
            CancelCommand = cancelCommand;
        }
    }
}