using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork17.Model
{
    public class NavigateToPathMessage : ValueChangedMessage<string>
    {
        public NavigateToPathMessage(string value) : base(value) { }
    }
}
