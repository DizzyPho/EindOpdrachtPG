using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Managers
{
    public class MessageManager
    {
        public void Register<T>(object recipient, MessageHandler<object, T> handler)
            where T : class
        {
            WeakReferenceMessenger.Default.Register<T>(recipient, handler);
        }
        public void Send<T>(T message) where T : class
        {
            WeakReferenceMessenger.Default.Send<T>(message);
        }
    }
}
