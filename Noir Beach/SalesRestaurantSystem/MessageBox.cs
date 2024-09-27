using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesRestaurantSystem
{
    public class MessageBox
    {
        public static MessageBoxResult ShowEmergentMessage(string msg)
        {
            string newMsg = string.Empty;
            var msgType = GetMessageType(msg, out newMsg);
            return System.Windows.MessageBox.Show(newMsg, msgType.Item2, MessageBoxButton.OK, msgType.Item1);
        }

        public static MessageBoxResult ShowConfirmMessage(string msg)
        {
            string newMsg = string.Empty;
            var msgType = GetMessageType(msg, out newMsg);
            return System.Windows.MessageBox.Show(newMsg, msgType.Item2, MessageBoxButton.YesNo, msgType.Item1);
        }


        public static (MessageBoxImage, string) GetMessageType(string msg, out string newMsg)
        {
            newMsg = msg[2..];
            return msg[0] switch
            {
                'E' => (MessageBoxImage.Error, "Error Program Message"),
                'W' => (MessageBoxImage.Warning, "Warning Program Message"),
                'I' => (MessageBoxImage.Information, "Program Message"),
                _ => (MessageBoxImage.Question, "Unclassified Program Message"),
            };
        }
        public static bool IsMessageError(string msg){ return msg[0] == 'E'; }
    }
}
