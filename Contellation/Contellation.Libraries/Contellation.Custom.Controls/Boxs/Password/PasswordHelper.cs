using System.Windows.Controls;

namespace Contellation.Custom.Controls
{
    public partial class PasswordBox
    {
        private class PasswordHelper
        {
            private readonly PasswordBox _owner;

            public PasswordHelper(PasswordBox owner)
            {
                _owner = owner;
            }

            public string GetNewPassword(ICollection<TextChange> changes)
            {
                string current = _owner.Password ?? "";
                string result = current;

                foreach (var change in changes)
                {
                    if (change.RemovedLength > 0)
                    {
                        result = result.Remove(change.Offset, change.RemovedLength);
                    }
                    if (change.AddedLength > 0)
                    {
                        string added = _owner.Text.Substring(change.Offset, change.AddedLength);
                        result = result.Insert(change.Offset, added);
                    }
                }

                return result;
            }
        }
    }
}
