using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NDG.Common
{
    public enum AuthentificationCode
    {
        InvalidCredentials = 1,
        ServerNotFound = 2,
        LoginSuccessed = 3,
        NoInternetConnection = 4,
    }
}
