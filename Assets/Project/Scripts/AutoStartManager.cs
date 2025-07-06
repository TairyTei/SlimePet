using Microsoft.Win32;
using UnityEngine;

public class AutoStartManager
{
    private const string AppName = "SlimePet"; 

    public static bool IsAutoStartEnabled()
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
        {
            return key?.GetValue(AppName) != null;
        }
    }
    public static void SetAutoStart(bool enabled)
    {
        string appPath = Application.dataPath.Replace("/_Data", ".exe");

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
        {
            if (enabled)
            {
                key.SetValue(AppName, $"\"{appPath}\"");
            }
            else
            {
                key.DeleteValue(AppName, false);
            }
        }
    }
}