using System;
using System.Collections.Generic;
using System.Text;
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
                // Logic xử lý thay đổi text khi ở chế độ ẩn
                // (có thể giữ hoặc tối ưu thêm theo nhu cầu)
                return _owner.Password; // tạm thời
            }
        }
    }
}
